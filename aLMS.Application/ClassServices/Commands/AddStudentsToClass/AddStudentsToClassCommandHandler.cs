using aLMS.Application.Common.Constants;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AccountEntity;
using aLMS.Domain.ParentProfileEntity;
using aLMS.Domain.StudentProfileEntity;
using aLMS.Domain.UserEntity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.AddStudentsToClass
{
    // AddStudentsToClassCommandHandler.cs
    public class AddStudentsToClassCommandHandler : IRequestHandler<AddStudentsToClassCommand, AddStudentsToClassResult>
    {
        private readonly IClassRepository _classRepo;
        private readonly IUsersRepository _userRepo;
        private readonly IAccountRepository _accRepo;
        private readonly IStudentProfileRepository _studentRepo;
        private readonly IParentProfileRepository _parentRepo;
        private readonly IRoleRepository _roleRepo;

        public AddStudentsToClassCommandHandler(
            IClassRepository classRepo,
            IUsersRepository userRepo,
            IAccountRepository accRepo,
            IStudentProfileRepository studentRepo,
            IParentProfileRepository parentRepo,
            IRoleRepository roleRepo)
        {
            _classRepo = classRepo;
            _userRepo = userRepo;
            _accRepo = accRepo;
            _studentRepo = studentRepo;
            _parentRepo = parentRepo;
            _roleRepo = roleRepo;
        }

        public async Task<AddStudentsToClassResult> Handle(AddStudentsToClassCommand request, CancellationToken ct)
        {
            var result = new AddStudentsToClassResult();

            var cls = await _classRepo.GetClassByIdAsync(request.ClassId);
            if (cls == null || cls.IsDeleted)
            {
                result.Success = false;
                result.Message = "Lớp không tồn tại hoặc đã bị xóa";
                return result;
            }

            // Lấy RoleId cố định
            var studentRoleId = await _roleRepo.GetRoleIdByNameAsync(RoleConstants.Student);
            var parentRoleId = await _roleRepo.GetRoleIdByNameAsync(RoleConstants.Parent);

            if (!studentRoleId.HasValue || !parentRoleId.HasValue)
            {
                result.Success = false;
                result.Message = "Không tìm thấy vai trò Student hoặc Parent";
                return result;
            }

            // Tạo mã lớp: 2410A1 → yearPrefix = 24, classCode = 10A1
            var yearPrefix = cls.SchoolYear.Split('-')[0][^2..]; // "2024-2025" → "24"
            var classCode = cls.ClassName.Replace(" ", "").ToUpper();

            // Lấy STT tiếp theo trong lớp
            int nextStt = await _studentRepo.GetMaxStudentOrderInClass(request.ClassId) + 1;

            foreach (var dto in request.Students)
            {
                try
                {
                    var studentDob = dto.StudentDateOfBirth.Date;
                    var ParentDob = dto.ParentDateOfBirth.Date;
                    var studentPassword = studentDob.ToString("ddMMyyyy"); // 15092012
                    var ParentPassword = ParentDob.ToString("ddMMyyyy"); // 15092012
                    var studentUsername = $"{yearPrefix}{classCode}{nextStt:D4}"; // 2410A1001
                    var parentPhone = dto.ParentPhone.Trim().Replace(" ", "").Replace("-", "");

                    // Kiểm tra trùng username
                    if (await _accRepo.UsernameExistsAsync(studentUsername))
                        throw new Exception($"Username học sinh {studentUsername} đã tồn tại");

                    if (string.IsNullOrWhiteSpace(parentPhone))
                        throw new Exception("Số điện thoại phụ huynh không được để trống");

                    if (await _accRepo.UsernameExistsAsync(parentPhone))
                        throw new Exception($"Số điện thoại phụ huynh {parentPhone} đã được dùng làm tài khoản");

                    // === TẠO ACCOUNT + USER HỌC SINH ===
                    var studentAccount = new Account
                    {
                        Id = Guid.NewGuid(),
                        Username = studentUsername,
                        Password = BCrypt.Net.BCrypt.HashPassword(studentPassword),
                        Status = true
                    };
                    await _accRepo.AddAsync(studentAccount);

                    var studentUser = new User
                    {
                        Id = Guid.NewGuid(),
                        AccountId = studentAccount.Id,
                        Name = dto.StudentName.Trim(),
                        DateOfBirth = studentDob,
                        Gender = dto.Gender ?? "Nam",
                        RoleId = studentRoleId.Value,
                        SchoolId = dto.SchoolId,
                        //PhoneNumber = parentPhone,
                        //Email = string.IsNullOrWhiteSpace(dto.ParentEmail) ? null : dto.ParentEmail.Trim(),
                        Address = dto.Address,

                    };
                    await _userRepo.AddAsync(studentUser);

                    // === TẠO STUDENT PROFILE ===
                    var studentProfile = new StudentProfile
                    {
                        UserId = studentUser.Id,
                        SchoolId = dto.SchoolId,
                        ClassId = cls.Id,
                        EnrollDate = dto.StudentEnrollDate.Date // Dùng ngày nhập học từ DTO
                    };
                    await _studentRepo.AddAsync(studentProfile);

                    // === TẠO ACCOUNT + USER PHỤ HUYNH ===
                    var parentDob = dto.ParentDateOfBirth.Date;
                    var parentPassword = ParentPassword; 

                    var parentAccount = new Account
                    {
                        Id = Guid.NewGuid(),
                        Username = parentPhone,
                        Password = BCrypt.Net.BCrypt.HashPassword(parentPassword),
                        Status = true
                    };
                    await _accRepo.AddAsync(parentAccount);

                    var parentUser = new User
                    {
                        Id = Guid.NewGuid(),
                        AccountId = parentAccount.Id,
                        Name = dto.ParentName.Trim(),
                        DateOfBirth = parentDob,
                        Gender = dto.ParentGender ?? "Nữ",
                        PhoneNumber = parentPhone,
                        Email = string.IsNullOrWhiteSpace(dto.ParentEmail) ? null : dto.ParentEmail.Trim(),
                        Address = dto.Address,
                        RoleId = parentRoleId.Value,
                        SchoolId = dto.SchoolId
                    };
                    await _userRepo.AddAsync(parentUser);

                    // === TẠO PARENT PROFILE ===
                    var parentProfile = new ParentProfile
                    {
                        UserId = parentUser.Id,
                        StudentId = studentUser.Id
                    };
                    await _parentRepo.AddAsync(parentProfile);

                    // Ghi kết quả trả về
                    result.CreatedStudents.Add(new StudentCreationResult
                    {
                        StudentName = dto.StudentName,
                        StudentUsername = studentUsername,
                        StudentPassword = studentPassword,
                        ParentName = dto.ParentName,
                        ParentUsername = parentPhone,
                        ParentPassword = parentPassword
                    });

                    nextStt++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"{dto.StudentName}: {ex.Message}");
                }
            }

            result.Success = result.Errors.Count == 0;
            result.Message = result.Success
                ? $"Thêm thành công {result.SuccessCount} học sinh vào lớp {cls.ClassName}"
                : $"Thành công: {result.SuccessCount}, Lỗi: {result.ErrorCount}";

            return result;
        }
    }
}
