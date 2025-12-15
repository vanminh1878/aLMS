using aLMS.Application.ClassServices.Commands.CreateClass;
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

            var existingClass = await _classRepository.GetClassByIdAsync(dto.Id);
            if (existingClass == null)
            {
                return new UpdateClassResult { Success = false, Message = "Lớp học không tồn tại." };
            }

            bool isNameChanged = !string.IsNullOrWhiteSpace(dto.ClassName)
                                 && dto.ClassName.Trim() != existingClass.ClassName;

            // Chỉ kiểm tra trùng tên nếu người dùng thực sự thay đổi tên lớp
            if (isNameChanged)
            {
                var classNameExists = await _classRepository.ClassNameExistsAsync(dto.ClassName.Trim(), dto.Id); 
                if (classNameExists)
                {
                    return new UpdateClassResult
                    {
                        Success = false,
                        Message = "Tên lớp đã tồn tại"
                    };
                }
            }

            // Ánh xạ có chọn lọc: chỉ update những field được gửi (không null/empty)
            if (!string.IsNullOrWhiteSpace(dto.ClassName))
                existingClass.ClassName = dto.ClassName.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Grade))
                existingClass.Grade = dto.Grade.Trim();

            if (!string.IsNullOrWhiteSpace(dto.SchoolYear))
                existingClass.SchoolYear = dto.SchoolYear.Trim();

            // Chỉ update IsDeleted nếu được gửi
            if (dto.IsDelete.HasValue)
            {
                if (dto.IsDelete.Value)
                    existingClass.SoftDelete(); // dùng method domain để raise event đúng
                else
                    existingClass.Restore();
            }
            existingClass.SchoolId = dto.SchoolId;
            if(dto.HomeroomTeacherId != null)
            {
                existingClass.HomeroomTeacherId = dto.HomeroomTeacherId;
            }

            try
            {
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