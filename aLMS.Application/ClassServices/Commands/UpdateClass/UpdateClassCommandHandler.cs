using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.UpdateClass
{
    public class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, UpdateClassResult>
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public UpdateClassCommandHandler(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<UpdateClassResult> Handle(UpdateClassCommand request, CancellationToken ct)
        {
            var dto = request.ClassDto;

            // Bước 1: Lấy entity hiện có từ DB (rất quan trọng!)
            var existingClass = await _classRepository.GetClassByIdAsync(dto.Id);
            if (existingClass == null)
            {
                return new UpdateClassResult
                {
                    Success = false,
                    Message = "Lớp học không tồn tại."
                };
            }

            // Bước 2: Map dữ liệu từ DTO đè lên entity cũ (giữ nguyên Tracking)
            _mapper.Map(dto, existingClass);

            try
            {
                // Bước 3: Raise event + Update
                existingClass.RaiseClassUpdatedEvent();
                await _classRepository.UpdateClassAsync(existingClass);

                return new UpdateClassResult
                {
                    Success = true,
                    Message = "Cập nhật lớp học thành công.",
                    ClassId = existingClass.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateClassResult
                {
                    Success = false,
                    Message = $"Lỗi không xác định: {ex.Message}"
                };
            }
        }
    }
}