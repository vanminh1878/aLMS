using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.RoleEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.RoleServices.Commands.CreateRole
{
    public class CreateRoleResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? RoleId { get; set; }
    }

    public class CreateRoleCommand : IRequest<CreateRoleResult>
    {
        public CreateRoleDto RoleDto { get; set; } = null!;
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleResult>
    {
        private readonly IRoleRepository _repo;
        private readonly IMapper _mapper;

        public CreateRoleCommandHandler(IRoleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CreateRoleResult> Handle(CreateRoleCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.RoleNameExistsAsync(request.RoleDto.RoleName);
                if (exists)
                    return new CreateRoleResult { Success = false, Message = "Role name already exists." };

                var role = _mapper.Map<Role>(request.RoleDto);
                role.Id = Guid.NewGuid();
                role.RaiseRoleCreatedEvent();
                await _repo.AddRoleAsync(role);

                return new CreateRoleResult
                {
                    Success = true,
                    Message = "Role created successfully.",
                    RoleId = role.Id
                };
            }
            catch (Exception ex)
            {
                return new CreateRoleResult
                {
                    Success = false,
                    Message = $"Error creating role: {ex.Message}"
                };
            }
        }
    }
}