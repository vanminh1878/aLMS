using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentProfileEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentProfileServices.Commands.UpdateStudentProfile
{
    public class UpdateStudentProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }

    public class UpdateStudentProfileCommand : IRequest<UpdateStudentProfileResult>
    {
        public UpdateStudentProfileDto Dto { get; set; } = null!;
    }

    public class UpdateStudentProfileCommandHandler : IRequestHandler<UpdateStudentProfileCommand, UpdateStudentProfileResult>
    {
        private readonly IStudentProfileRepository _profileRepo;
        private readonly IUsersRepository _usersRepo;
        private readonly IMapper _mapper;

        public UpdateStudentProfileCommandHandler(
            IStudentProfileRepository profileRepo,
            IUsersRepository usersRepo,
            IMapper mapper)
        {
            _profileRepo = profileRepo;
            _usersRepo = usersRepo;
            _mapper = mapper;
        }

        public async Task<UpdateStudentProfileResult> Handle(UpdateStudentProfileCommand request, CancellationToken ct)
        {
            try
            {
                var profile = await _profileRepo.GetByUserIdAsync(request.Dto.UserId);
                if (profile == null)
                    return new UpdateStudentProfileResult { Success = false, Message = "Student profile not found." };

                var user = await _usersRepo.GetByIdAsync(request.Dto.UserId);
                if (user == null)
                    return new UpdateStudentProfileResult { Success = false, Message = "User not found." };

                _mapper.Map(request.Dto, profile);
                profile.RaiseStudentProfileUpdatedEvent();
                await _profileRepo.UpdateAsync(profile);

                return new UpdateStudentProfileResult
                {
                    Success = true,
                    Message = "Student profile updated successfully.",
                    UserId = profile.UserId
                };
            }
            catch (Exception ex)
            {
                return new UpdateStudentProfileResult
                {
                    Success = false,
                    Message = $"Error updating profile: {ex.Message}"
                };
            }
        }
    }
}