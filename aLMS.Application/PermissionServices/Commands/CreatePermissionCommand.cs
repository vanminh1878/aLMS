// aLMS.Application.PermissionServices.Commands.CreatePermission/CreatePermissionCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.PermissionEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.PermissionServices.Commands.CreatePermission
{
    public class CreatePermissionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? PermissionId { get; set; }
    }

    public class CreatePermissionCommand : IRequest<CreatePermissionResult>
    {
        public CreatePermissionDto PermissionDto { get; set; } = null!;
    }

    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, CreatePermissionResult>
    {
        private readonly IPermissionRepository _repo;
        private readonly IMapper _mapper;

        public CreatePermissionCommandHandler(IPermissionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CreatePermissionResult> Handle(CreatePermissionCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.PermissionNameExistsAsync(request.PermissionDto.PermissionName);
                if (exists)
                    return new CreatePermissionResult { Success = false, Message = "Permission name already exists." };

                var permission = _mapper.Map<Permission>(request.PermissionDto);
                permission.Id = Guid.NewGuid();
                permission.RaisePermissionCreatedEvent();
                await _repo.AddPermissionAsync(permission);

                return new CreatePermissionResult
                {
                    Success = true,
                    Message = "Permission created successfully.",
                    PermissionId = permission.Id
                };
            }
            catch (Exception ex)
            {
                return new CreatePermissionResult
                {
                    Success = false,
                    Message = $"Error creating permission: {ex.Message}"
                };
            }
        }
    }
}