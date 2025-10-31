// aLMS.Application.ParentProfileServices.Commands.DeleteParentProfile/DeleteParentProfileCommand.cs
using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ParentProfileServices.Commands.DeleteParentProfile
{
    public class DeleteParentProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public Guid? StudentId { get; set; }
    }

    public class DeleteParentProfileCommand : IRequest<DeleteParentProfileResult>
    {
        public Guid ParentId { get; set; }
        public Guid StudentId { get; set; }
    }

    public class DeleteParentProfileCommandHandler : IRequestHandler<DeleteParentProfileCommand, DeleteParentProfileResult>
    {
        private readonly IParentProfileRepository _profileRepo;

        public DeleteParentProfileCommandHandler(IParentProfileRepository profileRepo) => _profileRepo = profileRepo;

        public async Task<DeleteParentProfileResult> Handle(DeleteParentProfileCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _profileRepo.ExistsAsync(request.ParentId, request.StudentId);
                if (!exists)
                    return new DeleteParentProfileResult { Success = false, Message = "Parent-Student link not found.", ParentId = request.ParentId, StudentId = request.StudentId };

                await _profileRepo.DeleteAsync(request.ParentId, request.StudentId);
                return new DeleteParentProfileResult
                {
                    Success = true,
                    Message = "Parent profile deleted successfully.",
                    ParentId = request.ParentId,
                    StudentId = request.StudentId
                };
            }
            catch (Exception ex)
            {
                return new DeleteParentProfileResult
                {
                    Success = false,
                    Message = $"Error deleting parent profile: {ex.Message}",
                    ParentId = request.ParentId,
                    StudentId = request.StudentId
                };
            }
        }
    }
}