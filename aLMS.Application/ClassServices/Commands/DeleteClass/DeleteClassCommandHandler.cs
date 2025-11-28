using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassEntity;
using MediatR;

namespace aLMS.Application.ClassServices.Commands.DeleteClass
{
    public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, DeleteClassResult>
    {
        private readonly IClassRepository _classRepository;

        public DeleteClassCommandHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<DeleteClassResult> Handle(DeleteClassCommand request, CancellationToken ct)
        {
            var classEntity = await _classRepository.GetClassByIdAsync(request.Id);

            if (classEntity == null || classEntity.IsDeleted)
            {
                return new DeleteClassResult
                {
                    Success = false,
                    Message = "Lớp học không tồn tại hoặc đã bị khóa trước đó."
                };
            }

            try
            {
                classEntity.SoftDelete(); // Đánh dấu xóa + raise event

                // Dùng Update thay vì Delete
                await _classRepository.UpdateClassAsync(classEntity);

                return new DeleteClassResult
                {
                    Success = true,
                    Message = "Lớp học đã được khóa thành công (không thể sử dụng nữa)."
                };
            }
            catch (Exception ex)
            {
                return new DeleteClassResult
                {
                    Success = false,
                    Message = $"Lỗi khi khóa lớp: {ex.Message}"
                };
            }
        }
    }
}