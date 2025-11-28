using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.LessonServices.Commands.DeleteLesson
{
    public class DeleteLessonResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? LessonId { get; set; }
    }

    public class DeleteLessonCommand : IRequest<DeleteLessonResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteLessonCommandHandler : IRequestHandler<DeleteLessonCommand, DeleteLessonResult>
    {
        private readonly ILessonRepository _repo;

        public DeleteLessonCommandHandler(ILessonRepository repo)
        {
            _repo = repo;
        }

        public async Task<DeleteLessonResult> Handle(DeleteLessonCommand request, CancellationToken ct)
        {
            var exists = await _repo.LessonExistsAsync(request.Id);
            if (!exists)
            {
                return new DeleteLessonResult
                {
                    Success = false,
                    Message = "Lesson not found.",
                    LessonId = request.Id
                };
            }

            try
            {
                // Gợi ý: Nếu muốn raise event trước khi xóa, cần lấy entity trước
                // Nhưng vì đang xóa cứng, nên không cần raise event ở đây
                // Nếu muốn → lấy entity → raise → xóa

                await _repo.DeleteLessonAsync(request.Id);

                return new DeleteLessonResult
                {
                    Success = true,
                    Message = "Lesson deleted successfully.",
                    LessonId = request.Id
                };
            }
            catch (Exception ex)
            {
                return new DeleteLessonResult
                {
                    Success = false,
                    Message = $"Error deleting lesson: {ex.Message}",
                    LessonId = request.Id
                };
            }
        }
    }
}