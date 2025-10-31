// aLMS.Application.ParentProfileServices.Commands.UpdateParentProfile/UpdateParentProfileCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ParentProfileEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ParentProfileServices.Commands.UpdateParentProfile
{
    public class UpdateParentProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public Guid? StudentId { get; set; }
    }

    public class UpdateParentProfileCommand : IRequest<UpdateParentProfileResult>
    {
        public UpdateParentProfileDto Dto { get; set; } = null!;
    }

    public class UpdateParentProfileCommandHandler : IRequestHandler<UpdateParentProfileCommand, UpdateParentProfileResult>
    {
        private readonly IParentProfileRepository _profileRepo;
        private readonly IUsersRepository _usersRepo;
        private readonly IMapper _mapper;

        public UpdateParentProfileCommandHandler(
            IParentProfileRepository profileRepo,
            IUsersRepository usersRepo,
            IMapper mapper)
        {
            _profileRepo = profileRepo;
            _usersRepo = usersRepo;
            _mapper = mapper;
        }

        public async Task<UpdateParentProfileResult> Handle(UpdateParentProfileCommand request, CancellationToken ct)
        {
            try
            {
                var profile = await _profileRepo.GetByParentIdAsync(request.Dto.ParentId);
                if (profile == null)
                    return new UpdateParentProfileResult { Success = false, Message = "Parent profile not found." };

                var student = await _usersRepo.GetByIdAsync(request.Dto.StudentId);
                if (student == null)
                    return new UpdateParentProfileResult { Success = false, Message = "Student not found." };

                if (profile.StudentId != request.Dto.StudentId)
                {
                    var exists = await _profileRepo.ExistsAsync(request.Dto.ParentId, request.Dto.StudentId);
                    if (exists)
                        return new UpdateParentProfileResult { Success = false, Message = "This parent is already linked to the new student." };
                }

                profile.StudentId = request.Dto.StudentId;
                profile.RaiseParentProfileUpdatedEvent();
                await _profileRepo.UpdateAsync(profile);

                return new UpdateParentProfileResult
                {
                    Success = true,
                    Message = "Parent profile updated successfully.",
                    ParentId = profile.UserId,
                    StudentId = profile.StudentId
                };
            }
            catch (Exception ex)
            {
                return new UpdateParentProfileResult
                {
                    Success = false,
                    Message = $"Error updating parent profile: {ex.Message}"
                };
            }
        }
    }
}