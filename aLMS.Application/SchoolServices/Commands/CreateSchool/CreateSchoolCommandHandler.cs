using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AccountEntity;
using aLMS.Domain.SchoolEntity;
using aLMS.Domain.UserEntity;
using AutoMapper;
using MediatR;
using BCrypt.Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Commands.CreateSchool
{
    public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, Guid>
    {
        private readonly ISchoolRepository _schoolRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IUsersRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSchoolCommandHandler(
            ISchoolRepository schoolRepo,
            IAccountRepository accountRepo,
            IUsersRepository userRepo,
            IRoleRepository roleRepo,
            IUnitOfWork unitOfWork)
        {
            _schoolRepo = schoolRepo;
            _accountRepo = accountRepo;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateSchoolCommand request, CancellationToken ct)
        {
            var dto = request.SchoolDto;

            if (dto.RoleId == null || dto.RoleId == Guid.Empty)
                throw new Exception("RoleId là bắt buộc.");

            if (string.IsNullOrWhiteSpace(dto.AdminUsername))
                throw new Exception("Tên đăng nhập không được để trống.");

            if (string.IsNullOrWhiteSpace(dto.AdminPassword))
                throw new Exception("Mật khẩu không được để trống.");

            var roleExists = await _roleRepo.RoleExistsAsync(dto.RoleId);
            if (!roleExists)
                throw new Exception("Role không tồn tại.");

            var usernameExists = await _accountRepo.UsernameExistsAsync(dto.AdminUsername);
            if (usernameExists)
                throw new Exception("Tên đăng nhập đã được sử dụng.");

            if (!string.IsNullOrEmpty(dto.AdminEmail))
            {
                var emailExists = await _userRepo.ExistsByEmailAsync(dto.AdminEmail);
                if (emailExists)
                    throw new Exception("Email đã được sử dụng.");
            }

            await _unitOfWork.BeginTransactionAsync(ct);

            try
            {
                var school = new School
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Address = dto.Address,
                    Email = dto.Email,
                    Status = true,
                };
                school.RaiseSchoolCreatedEvent();
                await _schoolRepo.AddSchoolAsync(school);

                var account = new Account
                {
                    Id = Guid.NewGuid(),
                    Username = dto.AdminUsername,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.AdminPassword),
                    Status = true,
                };
                account.RaiseAccountCreatedEvent();
                await _accountRepo.AddAsync(account);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = dto.AdminName,
                    Email = dto.AdminEmail,
                    PhoneNumber = dto.AdminPhone,
                    DateOfBirth = dto.AdminDateOfBirth,
                    Address = dto.AdminAddress,
                    Gender = dto.AdminGender,
                    SchoolId = school.Id,
                    AccountId = account.Id,
                    RoleId = dto.RoleId,
                };
                user.RaiseUserCreatedEvent();
                await _userRepo.AddAsync(user);

                await _unitOfWork.SaveChangesAsync(ct);

                await _unitOfWork.CommitTransactionAsync(ct);

                return school.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }
        }
    }
}