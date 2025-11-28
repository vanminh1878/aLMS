using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ParentProfileEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ParentProfileServices.Commands.CreateParentProfile
{
    public class CreateParentProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public Guid? StudentId { get; set; }
    }

    public class CreateParentProfileCommand : IRequest<CreateParentProfileResult>
    {
        public CreateParentProfileDto Dto { get; set; } = null!;
    }

    public class CreateParentProfileCommandHandler : IRequestHandler<CreateParentProfileCommand, CreateParentProfileResult>
    {
        private readonly IParentProfileRepository _profileRepo;
        private readonly IUsersRepository _usersRepo;
        private readonly IMapper _mapper;

        public CreateParentProfileCommandHandler(
            IParentProfileRepository profileRepo,
            IUsersRepository usersRepo,
            IMapper mapper)
        {
            _profileRepo = profileRepo;
            _usersRepo = usersRepo;
            _mapper = mapper;
        }

        public async Task<CreateParentProfileResult> Handle(CreateParentProfileCommand request, CancellationToken ct)
        {
            try
            {
                var parent = await _usersRepo.GetByIdAsync(request.Dto.ParentId);
                if (parent == null)
                    return new CreateParentProfileResult { Success = false, Message = "Parent not found." };

                var student = await _usersRepo.GetByIdAsync(request.Dto.StudentId);
                if (student == null)
                    return new CreateParentProfileResult { Success = false, Message = "Student not found." };

                var exists = await _profileRepo.ExistsAsync(request.Dto.ParentId, request.Dto.StudentId);
                if (exists)
                    return new CreateParentProfileResult { Success = false, Message = "Parent-Student link already exists." };

                var profile = _mapper.Map<ParentProfile>(request.Dto);
                profile.RaiseParentProfileCreatedEvent();
                await _profileRepo.AddAsync(profile);

                return new CreateParentProfileResult
                {
                    Success = true,
                    Message = "Parent profile created successfully.",
                    ParentId = profile.UserId,
                    StudentId = profile.StudentId
                };
            }
            catch (Exception ex)
            {
                return new CreateParentProfileResult
                {
                    Success = false,
                    Message = $"Error creating parent profile: {ex.Message}"
                };
            }
        }
    }
}