using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.TeacherProfileEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TeacherProfileServices.Commands.CreateTeacherProfile
{
    public class CreateTeacherProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }

    public class CreateTeacherProfileCommand : IRequest<CreateTeacherProfileResult>
    {
        public CreateTeacherProfileDto Dto { get; set; } = null!;
    }

    public class CreateTeacherProfileCommandHandler : IRequestHandler<CreateTeacherProfileCommand, CreateTeacherProfileResult>
    {
        private readonly ITeacherProfileRepository _profileRepo;
        private readonly IUsersRepository _usersRepo;
        private readonly IMapper _mapper;

        public CreateTeacherProfileCommandHandler(
            ITeacherProfileRepository profileRepo,
            IUsersRepository usersRepo,
            IMapper mapper)
        {
            _profileRepo = profileRepo;
            _usersRepo = usersRepo;
            _mapper = mapper;
        }

        public async Task<CreateTeacherProfileResult> Handle(CreateTeacherProfileCommand request, CancellationToken ct)
        {
            try
            {
                var user = await _usersRepo.GetByIdAsync(request.Dto.UserId);
                if (user == null)
                    return new CreateTeacherProfileResult { Success = false, Message = "User not found." };

                var exists = await _profileRepo.ExistsAsync(request.Dto.UserId);
                if (exists)
                    return new CreateTeacherProfileResult { Success = false, Message = "Teacher profile already exists." };

                var profile = _mapper.Map<TeacherProfile>(request.Dto);
                profile.RaiseTeacherProfileCreatedEvent();
                await _profileRepo.AddAsync(profile);

                return new CreateTeacherProfileResult
                {
                    Success = true,
                    Message = "Teacher profile created successfully.",
                    UserId = profile.UserId
                };
            }
            catch (Exception ex)
            {
                return new CreateTeacherProfileResult
                {
                    Success = false,
                    Message = $"Error creating profile: {ex.Message}"
                };
            }
        }
    }
}