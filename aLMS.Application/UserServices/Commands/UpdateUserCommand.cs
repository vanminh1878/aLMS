// aLMS.Application.UserServices.Commands.UpdateUser/UpdateUserCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.UserEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.UserServices.Commands.UpdateUser
{
    public class UpdateUserResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }

    public class UpdateUserCommand : IRequest<UpdateUserResult>
    {
        public UpdateUserDto Dto { get; set; } = null!;
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
    {
        private readonly IUsersRepository _repo;
        private readonly IRoleRepository _roleRepo;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUsersRepository repo, IRoleRepository roleRepo, IMapper mapper)
        {
            _repo = repo;
            _roleRepo = roleRepo;
            _mapper = mapper;
        }

        public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken ct)
        {
            try
            {
                var user = await _repo.GetByIdAsync(request.Dto.Id);
                if (user == null)
                    return new UpdateUserResult { Success = false, Message = "User not found." };

                if (!string.IsNullOrEmpty(request.Dto.Email) && request.Dto.Email != user.Email)
                {
                    var emailExists = await _repo.ExistsByEmailAsync(request.Dto.Email, request.Dto.Id);
                    if (emailExists)
                        return new UpdateUserResult { Success = false, Message = "Email already exists." };
                }

                if (request.Dto.RoleId.HasValue && request.Dto.RoleId != user.RoleId)
                {
                    var roleExists = await _roleRepo.RoleExistsAsync(request.Dto.RoleId.Value);
                    if (!roleExists)
                        return new UpdateUserResult { Success = false, Message = "Role not found." };
                }

                _mapper.Map(request.Dto, user);
                user.RaiseUserUpdatedEvent();
                await _repo.UpdateAsync(user);

                return new UpdateUserResult
                {
                    Success = true,
                    Message = "User updated successfully.",
                    UserId = user.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateUserResult
                {
                    Success = false,
                    Message = $"Error updating user: {ex.Message}"
                };
            }
        }
    }
}