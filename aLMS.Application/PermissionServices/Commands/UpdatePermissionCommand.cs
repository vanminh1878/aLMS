using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.PermissionEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.PermissionServices.Commands.UpdatePermission
{
    public class UpdatePermissionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? PermissionId { get; set; }
    }

    public class UpdatePermissionCommand : IRequest<UpdatePermissionResult>
    {
        public UpdatePermissionDto PermissionDto { get; set; } = null!;
    }

    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, UpdatePermissionResult>
    {
        private readonly IPermissionRepository _repo;
        private readonly IMapper _mapper;

        public UpdatePermissionCommandHandler(IPermissionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdatePermissionResult> Handle(UpdatePermissionCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.PermissionExistsAsync(request.PermissionDto.Id);
                if (!exists)
                    return new UpdatePermissionResult { Success = false, Message = "Permission not found." };

                var nameExists = await _repo.PermissionNameExistsAsync(request.PermissionDto.PermissionName, request.PermissionDto.Id);
                if (nameExists)
                    return new UpdatePermissionResult { Success = false, Message = "Permission name already exists." };

                var permission = _mapper.Map<Permission>(request.PermissionDto);
                permission.RaisePermissionUpdatedEvent();
                await _repo.UpdatePermissionAsync(permission);

                return new UpdatePermissionResult
                {
                    Success = true,
                    Message = "Permission updated successfully.",
                    PermissionId = permission.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdatePermissionResult
                {
                    Success = false,
                    Message = $"Error updating permission: {ex.Message}"
                };
            }
        }
    }
}