using aLMS.Application.Common.DTOs;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentProfileEntity;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.StudentExerciseServices.Queries
{
    public class GetClassExerciseOverviewQueryHandler : IRequestHandler<GetClassExerciseOverviewQuery, ClassExerciseOverviewDto>
    {
        private readonly IExerciseRepository _exerciseRepo;
        private readonly IClassRepository _classRepo;
        private readonly IStudentExerciseRepository _studentExerciseRepo;
        private readonly IStudentProfileRepository _studentProfileRepo;
        private readonly IUsersRepository _userRepo;
        private readonly IMapper _mapper;

        public GetClassExerciseOverviewQueryHandler(
            IExerciseRepository exerciseRepo,
            IClassRepository classRepo,
            IStudentExerciseRepository studentExerciseRepo,
            IUsersRepository userRepo,
            IStudentProfileRepository studentProfileRepo,
            IMapper mapper)
        {
            _exerciseRepo = exerciseRepo;
            _classRepo = classRepo;
            _studentExerciseRepo = studentExerciseRepo;
            _userRepo = userRepo;
            _studentProfileRepo = studentProfileRepo;
            _mapper = mapper;
        }

        public async Task<ClassExerciseOverviewDto> Handle(GetClassExerciseOverviewQuery request, CancellationToken ct)
        {
            // 1. Lấy thông tin Exercise
            var exercise = await _exerciseRepo.GetExerciseByIdAsync(request.ExerciseId);
            if (exercise == null) return null;

            // 2. Lấy thông tin Class
            var classEntity = await _classRepo.GetClassByIdAsync(request.ClassId);
            if (classEntity == null) return null;

            // 3. Lấy danh sách StudentProfile trong lớp
            var studentProfiles = await _studentProfileRepo.GetByClassIdAsync(request.ClassId);
            if (!studentProfiles.Any())
            {
                return new ClassExerciseOverviewDto
                {
                    ClassId = request.ClassId,
                    ClassName = classEntity.ClassName,
                    ExerciseTitle = exercise.Title,
                    TotalStudents = 0,
                    SubmittedCount = 0,
                    NotSubmittedCount = 0,
                    AverageScore = 0m,
                    HighestScore = 0m,
                    LowestScore = 0m,
                    StudentResults = new List<StudentExerciseSummaryDto>()
                };
            }

            var studentIds = studentProfiles.Select(sp => sp.UserId).ToList();

            // 4. Lấy tất cả StudentExercise của bài tập này từ các học sinh trong lớp
            var studentExercises = await _studentExerciseRepo
                .GetByExerciseAndStudentIdsAsync(request.ExerciseId, studentIds);

            // 5. Xác định học sinh đã nộp ít nhất một lần (có bản ghi IsCompleted = true)
            var submittedStudentIds = studentExercises
                .Where(se => se.IsCompleted)
                .Select(se => se.StudentId)
                .Distinct()
                .ToList();

            var submittedCount = submittedStudentIds.Count;
            var notSubmittedCount = studentProfiles.Count - submittedCount;

            // 6. Lấy lần làm bài mới nhất (hoặc tốt nhất) của từng học sinh để hiển thị và tính thống kê
            // Ở đây chọn lần làm mới nhất theo StartTime
            var representativeExercises = studentExercises
                .Where(se => se.IsCompleted) // chỉ lấy các lần đã hoàn thành
                .GroupBy(se => se.StudentId)
                .Select(g => g.OrderByDescending(se => se.StartTime).First()) // lần mới nhất
                .ToList();

            // 7. Tính toán thống kê dựa trên lần làm đại diện của từng học sinh
            var averageScore = representativeExercises.Any()
                ? Math.Round(representativeExercises.Average(se => se.Score), 2)
                : 0m;

            var highestScore = representativeExercises.Any()
                ? representativeExercises.Max(se => se.Score)
                : 0m;

            var lowestScore = representativeExercises.Any()
                ? representativeExercises.Min(se => se.Score)
                : 0m;

            // 8. Cache FullName từ UserRepository
            var userNameDict = new Dictionary<Guid, string>();

            async Task<string> GetFullNameAsync(Guid userId)
            {
                if (userNameDict.TryGetValue(userId, out var cachedName))
                {
                    return cachedName;
                }

                var user = await _userRepo.GetByIdAsync(userId);
                var fullName = user?.Name ?? "Không xác định";

                userNameDict[userId] = fullName;
                return fullName;
            }

            // 9. Build danh sách kết quả học sinh (mỗi học sinh chỉ xuất hiện 1 lần)
            var resultTasks = representativeExercises.Select(async se => new StudentExerciseSummaryDto
            {
                StudentExerciseId = se.Id,
                StudentId = se.StudentId,
                StudentName = await GetFullNameAsync(se.StudentId),
                StartTime = se.StartTime,
                EndTime = se.EndTime,
                Score = se.Score,
                TotalScore = exercise.TotalScore,
                IsCompleted = true,
                AttemptNumber = se.AttemptNumber
            });

            var notSubmittedTasks = studentProfiles
                .Where(sp => !submittedStudentIds.Contains(sp.UserId))
                .Select(async sp => new StudentExerciseSummaryDto
                {
                    StudentId = sp.UserId,
                    StudentName = await GetFullNameAsync(sp.UserId),
                    IsCompleted = false
                });

            var resultDtos = await Task.WhenAll(resultTasks);
            var notSubmittedDtos = await Task.WhenAll(notSubmittedTasks);

            var allResults = resultDtos
                .Concat(notSubmittedDtos)
                .OrderBy(x => x.StudentName)
                .ToList();

            // 10. Return DTO
            return new ClassExerciseOverviewDto
            {
                ClassId = request.ClassId,
                ClassName = classEntity.ClassName,
                ExerciseTitle = exercise.Title,
                TotalStudents = studentProfiles.Count,
                SubmittedCount = submittedCount,
                NotSubmittedCount = notSubmittedCount,
                AverageScore = averageScore,
                HighestScore = highestScore,
                LowestScore = lowestScore,
                StudentResults = allResults
            };
        }
    }
}