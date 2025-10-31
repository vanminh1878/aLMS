// aLMS.Application.UserServices.Commands.CreateUser/CreateUserCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AccountEntity;
using aLMS.Domain.UserEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.UserServices.Commands.CreateUser
{
    public class CreateUserResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
        public Guid? AccountId { get; set; }
    }

    public class CreateUserCommand : IRequest<CreateUserResult>
    {
        public CreateUserDto Dto { get; set; } = null!;
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
    {
        private readonly IUsersRepository _userRepo;
        private readonly IAccountRepository _accRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(
            IUsersRepository userRepo,
            IAccountRepository accRepo,
            IRoleRepository roleRepo,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _accRepo = accRepo;
            _roleRepo = roleRepo;
            _mapper = mapper;
        }

        public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken ct)
        {
            try
            {
                // Kiểm tra username
                var usernameExists = await _accRepo.UsernameExistsAsync(request.Dto.Username);
                if (usernameExists)
                    return new CreateUserResult { Success = false, Message = "Username already exists." };

                // Kiểm tra email
                if (!string.IsNullOrEmpty(request.Dto.Email))
                {
                    var emailExists = await _userRepo.ExistsByEmailAsync(request.Dto.Email);
                    if (emailExists)
                        return new CreateUserResult { Success = false, Message = "Email already exists." };
                }

                // Kiểm tra role
                if (request.Dto.RoleId.HasValue)
                {
                    var roleExists = await _roleRepo.RoleExistsAsync(request.Dto.RoleId.Value);
                    if (!roleExists)
                        return new CreateUserResult { Success = false, Message = "Role not found." };
                }

                // Tạo Account
                var account = new Account
                {
                    Id = Guid.NewGuid(),
                    Username = request.Dto.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Dto.Password),
                    Status = true
                };
                account.RaiseAccountCreatedEvent();
                await _accRepo.AddAsync(account);

                // Tạo User
                var user = _mapper.Map<User>(request.Dto);
                user.Id = Guid.NewGuid();
                user.AccountId = account.Id;
                user.RaiseUserCreatedEvent();
                await _userRepo.AddAsync(user);

                return new CreateUserResult
                {
                    Success = true,
                    Message = "User created successfully.",
                    UserId = user.Id,
                    AccountId = account.Id
                };
            }
            catch (Exception ex)
            {
                return new CreateUserResult
                {
                    Success = false,
                    Message = $"Error creating user: {ex.Message}"
                };
            }
        }
    }
}