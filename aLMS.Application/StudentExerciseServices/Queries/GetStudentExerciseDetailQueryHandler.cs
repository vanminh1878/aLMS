using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.DTOs;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentExerciseServices.Queries
{
    public class GetStudentExerciseDetailQuery : IRequest<StudentExerciseDetailDto?>
    {
        public Guid StudentExerciseId { get; set; }
    }

    public class GetStudentExerciseDetailQueryHandler
        : IRequestHandler<GetStudentExerciseDetailQuery, StudentExerciseDetailDto?>
    {
        private readonly IStudentExerciseRepository _studentExerciseRepo;
        private readonly IExerciseRepository _exerciseRepo;
        private readonly IStudentAnswerRepository _studentAnswerRepo;
        private readonly IUsersRepository _userRepo;
        private readonly IQuestionRepository _questionRepo;        // Thêm để lấy câu hỏi
        private readonly IAnswerRepository _answerRepo;            // Thêm để lấy đáp án đúng
        private readonly IMapper _mapper;

        public GetStudentExerciseDetailQueryHandler(
            IStudentExerciseRepository studentExerciseRepo,
            IExerciseRepository exerciseRepo,
            IStudentAnswerRepository studentAnswerRepo,
            IUsersRepository userRepo,
            IQuestionRepository questionRepo,
            IAnswerRepository answerRepo,
            IMapper mapper)
        {
            _studentExerciseRepo = studentExerciseRepo;
            _exerciseRepo = exerciseRepo;
            _studentAnswerRepo = studentAnswerRepo;
            _userRepo = userRepo;
            _questionRepo = questionRepo;
            _answerRepo = answerRepo;
            _mapper = mapper;
        }

        public async Task<StudentExerciseDetailDto?> Handle(GetStudentExerciseDetailQuery request, CancellationToken ct)
        {
            // 1. Lấy StudentExercise theo Id
            var studentExercise = await _studentExerciseRepo.GetByIdAsync(request.StudentExerciseId);
            if (studentExercise == null)
                return null;

            // 2. Lấy thông tin Exercise (tiêu đề, tổng điểm)
            var exercise = await _exerciseRepo.GetExerciseByIdAsync(studentExercise.ExerciseId);
            if (exercise == null)
                return null;

            // 3. Lấy danh sách câu trả lời của học sinh trong lần làm này
            var studentAnswers = await _studentAnswerRepo.GetByStudentExerciseIdAsync(request.StudentExerciseId);

            // 4. Lấy tên học sinh
            var user = await _userRepo.GetByIdAsync(studentExercise.StudentId);
            var studentName = user?.Name ?? "Không xác định";

            // 5. Lấy tất cả câu hỏi của bài tập + đáp án đúng (để hiển thị chi tiết)
            var questions = await _questionRepo.GetQuestionsByExerciseIdAsync(studentExercise.ExerciseId);
            var correctAnswerIds = questions.SelectMany(q => q.Answers.Where(a => a.IsCorrect).Select(a => a.Id))
                                           .ToList();

            // 6. Map sang DTO chi tiết
            var dto = _mapper.Map<StudentExerciseDetailDto>(studentExercise);

            dto.StudentName = studentName;
            dto.ExerciseTitle = exercise.Title;
            dto.TotalScore = exercise.TotalScore;

            // Map danh sách câu trả lời + bổ sung thông tin câu hỏi và đáp án đúng
            //dto.Answers = studentAnswers.Select(sa =>
            //{
            //    var answerDto = _mapper.Map<StudentAnswerDto>(sa);

            //    // Tìm câu hỏi tương ứng
            //    var question = questions.FirstOrDefault(q => q.Id == sa.QuestionId);
            //    if (question != null)
            //    {
            //        answerDto.QuestionContent = question.QuestionContent;
            //        answerDto.QuestionImage = question.QuestionImage;
            //        answerDto.QuestionType = question.QuestionType;
            //        answerDto.Score = question.Score; // Điểm của câu hỏi
            //    }


            //    var selectedAnswer = _answerRepo.GetByIdAsync(sa.AnswerId);
            //    answerDto.SelectedAnswerContent = selectedAnswer?.AnswerContent;


            //    // Đáp án đúng (có thể nhiều nếu đa chọn)
            //    var correctAnswers = question?.Answers.Where(a => a.IsCorrect).Select(a => a.AnswerContent).ToList();
            //    answerDto.CorrectAnswerContents = correctAnswers ?? new List<string>();

            //    // Explanation nếu có
            //    answerDto.Explanation = question?.Explanation;

            //    return answerDto;
            //}).ToList();
            dto.Answers = new List<StudentAnswerDto>();

            foreach (var sa in studentAnswers)
            {
                var answerDto = _mapper.Map<StudentAnswerDto>(sa);

                var question = questions.FirstOrDefault(q => q.Id == sa.QuestionId);
                if (question != null)
                {
                    answerDto.QuestionContent = question.QuestionContent;
                    answerDto.QuestionImage = question.QuestionImage;
                    answerDto.QuestionType = question.QuestionType;
                    answerDto.Score = question.Score;
                    answerDto.Explanation = question.Explanation;

                    answerDto.CorrectAnswerContents = question.Answers
                        .Where(a => a.IsCorrect)
                        .Select(a => a.AnswerContent)
                        .ToList();
                }

                var selectedAnswer = await _answerRepo.GetByIdAsync(sa.AnswerId);
                answerDto.SelectedAnswerContent = selectedAnswer?.AnswerContent;

                dto.Answers.Add(answerDto);
            }

            return dto;
        }
    }
}