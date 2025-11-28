using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentProfileServices.Commands.DeleteStudentProfile
{
    public class DeleteStudentProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }

    public class DeleteStudentProfileCommand : IRequest<DeleteStudentProfileResult>
    {
        public Guid UserId { get; set; }
    }

    public class DeleteStudentProfileCommandHandler : IRequestHandler<DeleteStudentProfileCommand, DeleteStudentProfileResult>
    {
        private readonly IStudentProfileRepository _profileRepo;

        public DeleteStudentProfileCommandHandler(IStudentProfileRepository profileRepo) => _profileRepo = profileRepo;

        public async Task<DeleteStudentProfileResult> Handle(DeleteStudentProfileCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _profileRepo.ExistsAsync(request.UserId);
                if (!exists)
                    return new DeleteStudentProfileResult { Success = false, Message = "Student profile not found.", UserId = request.UserId };

                await _profileRepo.DeleteAsync(request.UserId);
                return new DeleteStudentProfileResult
                {
                    Success = true,
                    Message = "Student profile deleted successfully.",
                    UserId = request.UserId
                };
            }
            catch (Exception ex)
            {
                return new DeleteStudentProfileResult
                {
                    Success = false,
                    Message = $"Error deleting profile: {ex.Message}",
                    UserId = request.UserId
                };
            }
        }
    }
}