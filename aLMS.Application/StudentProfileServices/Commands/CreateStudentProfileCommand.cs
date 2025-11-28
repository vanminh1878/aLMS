using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentProfileEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentProfileServices.Commands.CreateStudentProfile
{
    public class CreateStudentProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }

    public class CreateStudentProfileCommand : IRequest<CreateStudentProfileResult>
    {
        public CreateStudentProfileDto Dto { get; set; } = null!;
    }

    public class CreateStudentProfileCommandHandler : IRequestHandler<CreateStudentProfileCommand, CreateStudentProfileResult>
    {
        private readonly IStudentProfileRepository _profileRepo;
        private readonly IUsersRepository _usersRepo;
        private readonly IMapper _mapper;

        public CreateStudentProfileCommandHandler(
            IStudentProfileRepository profileRepo,
            IUsersRepository usersRepo,
            IMapper mapper)
        {
            _profileRepo = profileRepo;
            _usersRepo = usersRepo;
            _mapper = mapper;
        }

        public async Task<CreateStudentProfileResult> Handle(CreateStudentProfileCommand request, CancellationToken ct)
        {
            try
            {
                var user = await _usersRepo.GetByIdAsync(request.Dto.UserId);
                if (user == null)
                    return new CreateStudentProfileResult { Success = false, Message = "User not found." };

                var exists = await _profileRepo.ExistsAsync(request.Dto.UserId);
                if (exists)
                    return new CreateStudentProfileResult { Success = false, Message = "Student profile already exists." };

                var profile = _mapper.Map<StudentProfile>(request.Dto);
                profile.RaiseStudentProfileCreatedEvent();
                await _profileRepo.AddAsync(profile);

                return new CreateStudentProfileResult
                {
                    Success = true,
                    Message = "Student profile created successfully.",
                    UserId = profile.UserId
                };
            }
            catch (Exception ex)
            {
                return new CreateStudentProfileResult
                {
                    Success = false,
                    Message = $"Error creating profile: {ex.Message}"
                };
            }
        }
    }
}