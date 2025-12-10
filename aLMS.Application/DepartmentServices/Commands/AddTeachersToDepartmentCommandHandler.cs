// AddTeachersToDepartmentCommandHandler.cs
using aLMS.Application.Common.Constants;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AccountEntity;
using aLMS.Domain.TeacherProfileEntity;
using aLMS.Domain.UserEntity;
using MediatR;

public class AddTeachersToDepartmentCommandHandler
    : IRequestHandler<AddTeachersToDepartmentCommand, AddTeachersToDepartmentResult>
{
    private readonly IDepartmentRepository _deptRepo;
    private readonly IUsersRepository _userRepo;
    private readonly IAccountRepository _accRepo;
    private readonly ITeacherProfileRepository _teacherRepo;
    private readonly IRoleRepository _roleRepo;

    public AddTeachersToDepartmentCommandHandler(
        IDepartmentRepository deptRepo,
        IUsersRepository userRepo,
        IAccountRepository accRepo,
        ITeacherProfileRepository teacherRepo,
        IRoleRepository roleRepo)
    {
        _deptRepo = deptRepo;
        _userRepo = userRepo;
        _accRepo = accRepo;
        _teacherRepo = teacherRepo;
        _roleRepo = roleRepo;
    }

    public async Task<AddTeachersToDepartmentResult> Handle(AddTeachersToDepartmentCommand request, CancellationToken ct)
    {
        var result = new AddTeachersToDepartmentResult();

        var department = await _deptRepo.GetByIdAsync(request.DepartmentId);

        var teacherRoleId = await _roleRepo.GetRoleIdByNameAsync(RoleConstants.Teacher);
        if (!teacherRoleId.HasValue)
        {
            result.Success = false;
            result.Message = "Không tìm thấy vai trò Teacher";
            return result;
        }


        foreach (var dto in request.Teachers)
        {
            try
            {
                var dob = dto.DateOfBirth.Date;
                var password = dob.ToString("ddMMyyyy"); // Mật khẩu mặc định: ngày sinh ddMMyyyy

                var phone = dto.Phone.Trim().Replace(" ", "").Replace("-", "");
                if (string.IsNullOrWhiteSpace(phone))
                    throw new Exception("Số điện thoại không được để trống");

                if (await _accRepo.UsernameExistsAsync(phone))
                    throw new Exception($"Số điện thoại {phone} đã được dùng làm tài khoản");

                // Username = số điện thoại (dễ nhớ, phổ biến ở trường học)
                var username = phone;

                // Tạo Account
                var account = new Account
                {
                    Id = Guid.NewGuid(),
                    Username = username,
                    Password = BCrypt.Net.BCrypt.HashPassword(password),
                    Status = true
                };
                await _accRepo.AddAsync(account);

                // Tạo User
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

                // Tạo Teacher Profile
                var teacherProfile = new TeacherProfile
                {
                    UserId = user.Id,
                    DepartmentId = request.DepartmentId,
                    HireDate = dto.HireDate.Date,
                    Specialization = dto.Specialization?.Trim()
                };
                await _teacherRepo.AddAsync(teacherProfile);

                result.CreatedTeachers.Add(new TeacherCreationResult
                {
                    TeacherName = dto.FullName,
                    Username = username,
                    Password = password
                });
            }
            catch (Exception ex)
            {
                result.Errors.Add($"{dto.FullName}: {ex.Message}");
            }
        }

        result.Success = result.Errors.Count == 0;
        result.Message = result.Success
            ? $"Thêm thành công {result.SuccessCount} giáo viên vào tổ {department.DepartmentName}"
            : $"Thành công: {result.SuccessCount}, Lỗi: {result.ErrorCount}";

        return result;
    }
}