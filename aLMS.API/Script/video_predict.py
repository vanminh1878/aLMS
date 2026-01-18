from fastapi import FastAPI, File, UploadFile, HTTPException
from fastapi.responses import JSONResponse
import torch
import torch.nn as nn
from transformers import AutoModelForVideoClassification, Wav2Vec2Model
import cv2
import numpy as np
import librosa
from pydub import AudioSegment
import tempfile
import os
import traceback  # Thêm để in lỗi chi tiết
from torchvision import transforms as T
from fastapi.middleware.cors import CORSMiddleware

# Danh sách 10 hành vi (phải đúng thứ tự như khi train)
# Danh sách 10 hành vi - có cả tiếng Anh và tiếng Việt
behavior_labels = [
    {
        "en": "Absence or avoidance of eye contact",
        "vi": "Thiếu hoặc tránh giao tiếp bằng mắt"
    },
    {
        "en": "Aggressive behavior",
        "vi": "Hành vi hung hăng"
    },
    {
        "en": "Hyper- or hyporeactivity to sensory input",
        "vi": "Phản ứng quá mức hoặc quá ít với kích thích giác quan"
    },
    {
        "en": "Non-responsiveness to verbal interaction",
        "vi": "Không phản hồi với tương tác bằng lời nói"
    },
    {
        "en": "Non-typical language",
        "vi": "Ngôn ngữ không điển hình"
    },
    {
        "en": "Object lining-up",
        "vi": "Xếp hàng đồ vật"
    },
    {
        "en": "Self-hitting or self-injurious behavior",
        "vi": "Tự đánh hoặc tự làm đau bản thân"
    },
    {
        "en": "Self-spinning or spinning objects",
        "vi": "Tự xoay người hoặc xoay đồ vật"
    },
    {
        "en": "Upper limb stereotypies",
        "vi": "Hành vi lặp lại tay/vùng chi trên"
    },
    {
        "en": "Background",
        "vi": "Nền (không có hành vi đặc trưng)"
    }
]


num_labels = len(behavior_labels)  # 10

# Định nghĩa lại mô hình fusion
class TransformerFusion(nn.Module):
    def __init__(self, dim=512, num_heads=4, num_labels=10, dropout=0.5):
        super().__init__()
        self.video_fc = nn.Linear(768, dim)
        self.audio_fc = nn.Linear(768, dim)
        self.dropout = nn.Dropout(dropout)
        self.transformer = nn.TransformerEncoder(
            nn.TransformerEncoderLayer(d_model=dim, nhead=num_heads, dropout=dropout, batch_first=True),
            num_layers=2
        )
        self.fc = nn.Linear(dim, num_labels)
    
    def forward(self, video_features, audio_features):
        video_features = self.video_fc(video_features)
        audio_features = self.audio_fc(audio_features)
        video_features = self.dropout(video_features)
        audio_features = self.dropout(audio_features)
        combined = torch.stack([video_features, audio_features], dim=1)  # (B, 2, dim)
        transformer_output = self.transformer(combined)
        fused = transformer_output.mean(dim=1)
        output = self.fc(fused)
        return output

# FastAPI app
app = FastAPI(title="ASD Behavior Detection from Video")
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Trong dev dùng "*", production thay bằng domain thật (ví dụ: "http://localhost:3000")
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)
# Device
device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
print(f"Using device: {device}")

# Load models với kiểm tra lỗi chi tiết
def load_models(model_path="best_model.pth"):
    print(f"Đang load models từ: {os.path.abspath(model_path)}")
    
    if not os.path.exists(model_path):
        raise FileNotFoundError(f"Không tìm thấy file model: {model_path}. Vui lòng đặt file best_model.pth cùng thư mục với script.")

    try:
        # Timesformer
        timesformer = AutoModelForVideoClassification.from_pretrained(
            "facebook/timesformer-base-finetuned-k400",
            output_hidden_states=True,
            ignore_mismatched_sizes=True,
            num_labels=num_labels
        )
        
        # Wav2vec2
        wav2vec = Wav2Vec2Model.from_pretrained("facebook/wav2vec2-base")
        
        # Fusion
        fusion_model = TransformerFusion(num_labels=num_labels).to(device)
        
        # Load checkpoint
        checkpoint = torch.load(model_path, map_location=device)
        print("Các key trong checkpoint:", list(checkpoint.keys()))
        
        timesformer.load_state_dict(checkpoint['timesformer'])
        wav2vec.load_state_dict(checkpoint['wav2vec'])
        fusion_model.load_state_dict(checkpoint['fusion_model'])
        
        timesformer.eval()
        wav2vec.eval()
        fusion_model.eval()
        
        timesformer.to(device)
        wav2vec.to(device)
        
        print("Load models thành công!")
        return timesformer, wav2vec, fusion_model
        
    except Exception as e:
        print("LỖI KHI LOAD MODEL:")
        traceback.print_exc()
        raise RuntimeError(f"Không thể load model: {str(e)}")

# Load models khi khởi động
try:
    timesformer, wav2vec, fusion_model = load_models("best_model.pth")
except Exception as e:
    print(f"KHÔNG THỂ KHỞI ĐỘNG SERVER DO LỖI LOAD MODEL: {e}")
    raise

# Transform cho frames
video_transform = T.Compose([
    T.Normalize(mean=[0.485, 0.456, 0.406], std=[0.229, 0.224, 0.225]),
])

# Hàm xử lý audio - bắt lỗi rõ ràng
def extract_and_process_audio(video_bytes: bytes) -> torch.Tensor:
    tmp_video_path = None
    wav_path = None
    try:
        # Lưu video tạm
        with tempfile.NamedTemporaryFile(delete=False, suffix=".mp4") as tmp_video:
            tmp_video.write(video_bytes)
            tmp_video_path = tmp_video.name

        print(f"Đang trích xuất audio từ video tạm: {tmp_video_path}")

        # Trích audio bằng pydub
        audio_segment = AudioSegment.from_file(tmp_video_path)
        wav_path = tmp_video_path.replace(".mp4", ".wav")
        audio_segment.export(wav_path, format="wav")
        print("Export wav thành công")

        # Load và resample bằng librosa
        y, sr = librosa.load(wav_path, sr=None)
        print(f"Audio gốc: sr={sr}, length={len(y)} samples")
        
        y = librosa.resample(y, orig_sr=sr, target_sr=16000)
        
        # Pad/cắt về 5 giây
        target_length = 16000 * 5
        if len(y) < target_length:
            y = np.pad(y, (0, target_length - len(y)))
            print("Đã pad audio lên 5 giây")
        else:
            y = y[:target_length]
            print("Đã cắt audio về 5 giây")
        
        return torch.tensor(y, dtype=torch.float32)
        
    except Exception as e:
        print("LỖI KHI XỬ LÝ AUDIO:")
        traceback.print_exc()
        raise RuntimeError(f"Không thể trích xuất hoặc xử lý audio: {str(e)}")
        
    finally:
        # Dọn dẹp file tạm
        for path in [tmp_video_path, wav_path]:
            if path and os.path.exists(path):
                try:
                    os.unlink(path)
                except:
                    pass

# Hàm sample frames - bắt lỗi rõ ràng
def sample_frames_from_video(video_bytes: bytes, num_frames=16):
    tmp_path = None
    try:
        with tempfile.NamedTemporaryFile(delete=False, suffix=".mp4") as tmp:
            tmp.write(video_bytes)
            tmp_path = tmp.name

        print(f"Đang đọc video tạm: {tmp_path}")
        
        cap = cv2.VideoCapture(tmp_path)
        if not cap.isOpened():
            raise ValueError("Không thể mở video bằng OpenCV")
        
        frame_count = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))
        print(f"Tổng số frame: {frame_count}")
        
        if frame_count <= 0:
            raise ValueError("Video không có frame hợp lệ")
        
        indices = np.linspace(0, max(frame_count - 1, 0), num_frames, dtype=int)
        frames = []
        last_valid_frame = None
        
        for i in indices:
            cap.set(cv2.CAP_PROP_POS_FRAMES, i)
            ret, frame = cap.read()
            if ret:
                frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                frame = cv2.resize(frame, (224, 224))
                last_valid_frame = frame.copy()
            else:
                if last_valid_frame is None:
                    frame = np.zeros((224, 224, 3), dtype=np.uint8)
                else:
                    frame = last_valid_frame.copy()
            
            frame_tensor = torch.tensor(frame, dtype=torch.float32).permute(2, 0, 1) / 255.0
            frame_tensor = video_transform(frame_tensor)
            frames.append(frame_tensor)
        
        cap.release()
        
        video_tensor = torch.stack(frames)  # (16, 3, 224, 224)
        video_tensor = torch.stack(frames).unsqueeze(0)  # (1, 16, 3, 224, 224)
        print(f"Sample frames thành công: shape {video_tensor.shape}")
        
        return video_tensor
        
    except Exception as e:
        print("LỖI KHI XỬ LÝ VIDEO FRAMES:")
        traceback.print_exc()
        raise RuntimeError(f"Không thể xử lý frames video: {str(e)}")
        
    finally:
        if tmp_path and os.path.exists(tmp_path):
            try:
                os.unlink(tmp_path)
            except:
                pass

# @app.post("/predict/")
# async def predict(file: UploadFile = File(...)):
#     print(f"\n=== NHẬN REQUEST MỚI ===\nFile: {file.filename}, Content-Type: {file.content_type}")
    
#     if file.content_type not in ["video/mp4", "video/mpeg", "video/x-m4v"]:
#         raise HTTPException(status_code=400, detail="Chỉ hỗ trợ định dạng video MP4")

#     try:
#         video_bytes = await file.read()
#         print(f"Đã đọc video: {len(video_bytes) / (1024*1024):.1f} MB")
        
#         if len(video_bytes) == 0:
#             raise ValueError("File video rỗng")
        
#         # Xử lý video + audio
#         video_tensor = sample_frames_from_video(video_bytes)
#         video_tensor = video_tensor.to(device)
        
#         audio_tensor = extract_and_process_audio(video_bytes)
#         audio_tensor = audio_tensor.unsqueeze(0).to(device)  # (1, 80000)
        
#         # Inference
#         print("Bắt đầu inference...")
#         with torch.no_grad():
#             video_outputs = timesformer(pixel_values=video_tensor)
#             video_features = video_outputs.hidden_states[-1][:, 0, :]  # (1, 768)
            
#             audio_outputs = wav2vec(audio_tensor)
#             audio_features = audio_outputs.last_hidden_state[:, 0, :]  # (1, 768)
            
#             logits = fusion_model(video_features, audio_features)
#             probabilities = torch.sigmoid(logits).cpu().numpy()[0]
        
#         probabilities = probabilities.astype(float).tolist()
#         predicted_labels = [behavior_labels[i] for i, prob in enumerate(probabilities) if prob > 0.3]
        
#         result = {
#             "detected_behaviors": predicted_labels,
#             "all_probabilities": [
#                 {"behavior": behavior_labels[i], "probability": round(probabilities[i], 4)}
#                 for i in range(num_labels)
#             ]
#         }
        
#         print("DỰ ĐOÁN THÀNH CÔNG!")
#         print("Hành vi phát hiện (>0.3):", predicted_labels)
#         return JSONResponse(content=result)
        
#     except Exception as e:
#         print("\n=== LỖI 500 TRONG /predict/ ===")
#         traceback.print_exc()  # In toàn bộ traceback ra terminal
#         print("==================================\n")
#         raise HTTPException(status_code=500, detail=f"Lỗi xử lý video: {str(e)}")
@app.post("/predict/")
async def predict(file: UploadFile = File(...)):
    print(f"\n=== NHẬN REQUEST MỚI ===\nFile: {file.filename}, Content-Type: {file.content_type}")
    
    if file.content_type not in ["video/mp4", "video/mpeg", "video/x-m4v"]:
        raise HTTPException(status_code=400, detail="Chỉ hỗ trợ định dạng video MP4")

    try:
        video_bytes = await file.read()
        print(f"Đã đọc video: {len(video_bytes) / (1024*1024):.1f} MB")
        
        if len(video_bytes) == 0:
            raise ValueError("File video rỗng")
        
        # Xử lý video + audio (giữ nguyên phần này)
        video_tensor = sample_frames_from_video(video_bytes)
        video_tensor = video_tensor.to(device)
        
        audio_tensor = extract_and_process_audio(video_bytes)
        audio_tensor = audio_tensor.unsqueeze(0).to(device)
        
        # Inference
        print("Bắt đầu inference...")
        with torch.no_grad():
            video_outputs = timesformer(pixel_values=video_tensor)
            video_features = video_outputs.hidden_states[-1][:, 0, :]
            
            audio_outputs = wav2vec(audio_tensor)
            audio_features = audio_outputs.last_hidden_state[:, 0, :]
            
            logits = fusion_model(video_features, audio_features)
            probabilities = torch.sigmoid(logits).cpu().numpy()[0]
        
        probabilities = probabilities.astype(float).tolist()
        
        # Lọc các hành vi có xác suất > 0.3 (dùng nhãn tiếng Việt)
        predicted_labels_vi = [
            behavior_labels[i]["vi"] 
            for i, prob in enumerate(probabilities) 
            if prob > 0.3
        ]
        
        # Tất cả xác suất (cả tiếng Việt)
        all_probabilities_vi = [
            {
                "behavior": behavior_labels[i]["vi"],
                "probability": round(probabilities[i], 4)
            }
            for i in range(num_labels)
        ]
        
        result = {
            "detected_behaviors": predicted_labels_vi,
            "all_probabilities": all_probabilities_vi,
            # Nếu frontend cần cả tiếng Anh thì có thể thêm tùy chọn:
            # "detected_behaviors_en": [...],
            # "all_probabilities_en": [...]
        }
        
        print("DỰ ĐOÁN THÀNH CÔNG!")
        print("Hành vi phát hiện (>0.3):", predicted_labels_vi)
        return JSONResponse(content=result)
        
    except Exception as e:
        print("\n=== LỖI 500 TRONG /predict/ ===")
        traceback.print_exc()
        raise HTTPException(status_code=500, detail=f"Lỗi xử lý video: {str(e)}")
@app.get("/")
def root():
    return {"message": "ASD Behavior Detection API đang chạy. Upload video tại POST /predict/"}