using aLMS.Application.Common.Constants;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AccountEntity;
using aLMS.Domain.TeacherProfileEntity;
using aLMS.Domain.UserEntity;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.DepartmentServices.Commands.AddTeachersToDepartment // ← namespace nên phù hợp với dự án của bạn
{
    public class AddTeachersToDepartmentCommandHandler
        : IRequestHandler<AddTeachersToDepartmentCommand, AddTeachersToDepartmentResult>
    {
        private readonly IDepartmentRepository _deptRepo;
        private readonly IUsersRepository _userRepo;
        private readonly IAccountRepository _accRepo;
        private readonly ITeacherProfileRepository _teacherRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUnitOfWork _unitOfWork;           // ← Thêm vào đây

        public AddTeachersToDepartmentCommandHandler(
            IDepartmentRepository deptRepo,
            IUsersRepository userRepo,
            IAccountRepository accRepo,
            ITeacherProfileRepository teacherRepo,
            IRoleRepository roleRepo,
            IUnitOfWork unitOfWork)                         // ← Inject thêm
        {
            _deptRepo = deptRepo;
            _userRepo = userRepo;
            _accRepo = accRepo;
            _teacherRepo = teacherRepo;
            _roleRepo = roleRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddTeachersToDepartmentResult> Handle(
            AddTeachersToDepartmentCommand request,
            CancellationToken ct)
        {
            var result = new AddTeachersToDepartmentResult();

            var department = await _deptRepo.GetByIdAsync(request.DepartmentId);
            if (department == null)
            {
                result.Success = false;
                result.Message = "Tổ bộ môn không tồn tại";
                return result;
            }

            var teacherRoleId = await _roleRepo.GetRoleIdByNameAsync(RoleConstants.Teacher);
            if (!teacherRoleId.HasValue)
            {
                result.Success = false;
                result.Message = "Không tìm thấy vai trò Teacher";
                return result;
            }

            // BẮT ĐẦU TRANSACTION
            await _unitOfWork.BeginTransactionAsync(ct);

            try
            {
                foreach (var dto in request.Teachers)
                {
                    var dob = dto.DateOfBirth.Date;
                    var password = dob.ToString("ddMMyyyy");
                    var phone = dto.Phone.Trim().Replace(" ", "").Replace("-", "");

                    if (string.IsNullOrWhiteSpace(phone))
                        throw new Exception("Số điện thoại không được để trống");

                    if (await _accRepo.UsernameExistsAsync(phone))
                        throw new Exception($"Số điện thoại {phone} đã được sử dụng làm tài khoản");

                    // Username = số điện thoại (phổ biến cho giáo viên)
                    var username = phone;

                    // === TẠO ACCOUNT ===
                    var account = new Account
                    {
                        Id = Guid.NewGuid(),
                        Username = username,
                        Password = BCrypt.Net.BCrypt.HashPassword(password),
                        Status = true
                    };
                    await _accRepo.AddAsync(account);

                    // === TẠO USER ===
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        AccountId = account.Id,
                        Name = dto.FullName.Trim(),
                        DateOfBirth = dob,
                        Gender = dto.Gender ?? "Nam",
                        PhoneNumber = phone,
                        Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(),
                        Address = dto.Address?.Trim(),
                        RoleId = teacherRoleId.Value,
                        SchoolId = dto.SchoolId
                    };
                    await _userRepo.AddAsync(user);

                    // === TẠO TEACHER PROFILE ===
                    var teacherProfile = new TeacherProfile
                    {
                        UserId = user.Id,
                        DepartmentId = request.DepartmentId,
                        HireDate = dto.HireDate.Date,
                        Specialization = dto.Specialization?.Trim()
                    };
                    await _teacherRepo.AddAsync(teacherProfile);

                    // Ghi nhận thành công từng người
                    result.CreatedTeachers.Add(new TeacherCreationResult
                    {
                        TeacherName = dto.FullName,
                        Username = username,
                        Password = password
                    });
                }

                // Nếu đến đây → tất cả OK → commit
                await _unitOfWork.CommitTransactionAsync(ct);

                result.Success = true;
                result.Message = $"Thêm thành công {result.CreatedTeachers.Count} giáo viên vào tổ {department.DepartmentName}";
            }
            catch (Exception ex)
            {
                // Lỗi bất kỳ → rollback toàn bộ
                await _unitOfWork.RollbackTransactionAsync(ct);

                result.Success = false;
                result.Message = "Có lỗi xảy ra khi thêm giáo viên. Toàn bộ thao tác đã được hủy.";
                result.Errors.Add($"Lỗi tổng thể: {ex.Message}");

                // Nếu muốn chi tiết hơn có thể log ex.StackTrace ở đây
            }

            return result;
        }
    }
}