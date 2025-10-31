// aLMS.Application.RoleServices.Commands.UpdateRole/UpdateRoleCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.RoleEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.RoleServices.Commands.UpdateRole
{
    public class UpdateRoleResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? RoleId { get; set; }
    }

    public class UpdateRoleCommand : IRequest<UpdateRoleResult>
    {
        public UpdateRoleDto RoleDto { get; set; } = null!;
    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, UpdateRoleResult>
    {
        private readonly IRoleRepository _repo;
        private readonly IMapper _mapper;

        public UpdateRoleCommandHandler(IRoleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdateRoleResult> Handle(UpdateRoleCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.RoleExistsAsync(request.RoleDto.Id);
                if (!exists)
                    return new UpdateRoleResult { Success = false, Message = "Role not found." };

                var nameExists = await _repo.RoleNameExistsAsync(request.RoleDto.RoleName, request.RoleDto.Id);
                if (nameExists)
                    return new UpdateRoleResult { Success = false, Message = "Role name already exists." };

                var role = _mapper.Map<Role>(request.RoleDto);
                role.RaiseRoleUpdatedEvent();
                await _repo.UpdateRoleAsync(role);

                return new UpdateRoleResult
                {
                    Success = true,
                    Message = "Role updated successfully.",
                    RoleId = role.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateRoleResult
                {
                    Success = false,
                    Message = $"Error updating role: {ex.Message}"
                };
            }
        }
    }
}