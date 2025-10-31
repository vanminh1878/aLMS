// aLMS.Application.TeacherProfileServices.Commands.UpdateTeacherProfile/UpdateTeacherProfileCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.TeacherProfileEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TeacherProfileServices.Commands.UpdateTeacherProfile
{
    public class UpdateTeacherProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }

    public class UpdateTeacherProfileCommand : IRequest<UpdateTeacherProfileResult>
    {
        public UpdateTeacherProfileDto Dto { get; set; } = null!;
    }

    public class UpdateTeacherProfileCommandHandler : IRequestHandler<UpdateTeacherProfileCommand, UpdateTeacherProfileResult>
    {
        private readonly ITeacherProfileRepository _profileRepo;
        private readonly IUsersRepository _usersRepo;
        private readonly IMapper _mapper;

        public UpdateTeacherProfileCommandHandler(
            ITeacherProfileRepository profileRepo,
            IUsersRepository usersRepo,
            IMapper mapper)
        {
            _profileRepo = profileRepo;
            _usersRepo = usersRepo;
            _mapper = mapper;
        }

        public async Task<UpdateTeacherProfileResult> Handle(UpdateTeacherProfileCommand request, CancellationToken ct)
        {
            try
            {
                var profile = await _profileRepo.GetByUserIdAsync(request.Dto.UserId);
                if (profile == null)
                    return new UpdateTeacherProfileResult { Success = false, Message = "Teacher profile not found." };

                var user = await _usersRepo.GetByIdAsync(request.Dto.UserId);
                if (user == null)
                    return new UpdateTeacherProfileResult { Success = false, Message = "User not found." };

                _mapper.Map(request.Dto, profile);
                profile.RaiseTeacherProfileUpdatedEvent();
                await _profileRepo.UpdateAsync(profile);

                return new UpdateTeacherProfileResult
                {
                    Success = true,
                    Message = "Teacher profile updated successfully.",
                    UserId = profile.UserId
                };
            }
            catch (Exception ex)
            {
                return new UpdateTeacherProfileResult
                {
                    Success = false,
                    Message = $"Error updating profile: {ex.Message}"
                };
            }
        }
    }
}