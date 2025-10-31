// aLMS.Application.TeacherProfileServices.Commands.DeleteTeacherProfile/DeleteTeacherProfileCommand.cs
using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TeacherProfileServices.Commands.DeleteTeacherProfile
{
    public class DeleteTeacherProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }

    public class DeleteTeacherProfileCommand : IRequest<DeleteTeacherProfileResult>
    {
        public Guid UserId { get; set; }
    }

    public class DeleteTeacherProfileCommandHandler : IRequestHandler<DeleteTeacherProfileCommand, DeleteTeacherProfileResult>
    {
        private readonly ITeacherProfileRepository _profileRepo;

        public DeleteTeacherProfileCommandHandler(ITeacherProfileRepository profileRepo) => _profileRepo = profileRepo;

        public async Task<DeleteTeacherProfileResult> Handle(DeleteTeacherProfileCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _profileRepo.ExistsAsync(request.UserId);
                if (!exists)
                    return new DeleteTeacherProfileResult { Success = false, Message = "Teacher profile not found.", UserId = request.UserId };

                await _profileRepo.DeleteAsync(request.UserId);
                return new DeleteTeacherProfileResult
                {
                    Success = true,
                    Message = "Teacher profile deleted successfully.",
                    UserId = request.UserId
                };
            }
            catch (Exception ex)
            {
                return new DeleteTeacherProfileResult
                {
                    Success = false,
                    Message = $"Error deleting profile: {ex.Message}",
                    UserId = request.UserId
                };
            }
        }
    }
}