using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassSubjectEntity;
using aLMS.Domain.TimetableEntity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TimetableServices.Commands
{
    public class GenerateTimetableCommand : IRequest<GenerateTimetableResult>
    {
        public GenerateTimetableDto Dto { get; set; }
    }

    public class GenerateTimetableCommandHandler : IRequestHandler<GenerateTimetableCommand, GenerateTimetableResult>
    {
        private readonly ITimetableRepository _timetableRepo;
        private readonly IClassSubjectRepository _classSubjectRepo;
        private readonly IClassSubjectTeacherRepository _teacherAssignmentRepo;
        private readonly Random _random = new Random();

        public GenerateTimetableCommandHandler(
            ITimetableRepository timetableRepo,
            IClassSubjectRepository classSubjectRepo,
            IClassSubjectTeacherRepository teacherAssignmentRepo)
        {
            _timetableRepo = timetableRepo;
            _classSubjectRepo = classSubjectRepo;
            _teacherAssignmentRepo = teacherAssignmentRepo;
        }

        public async Task<GenerateTimetableResult> Handle(GenerateTimetableCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            var warnings = new List<string>();
            int assigned = 0;

            // Bước 1: Load môn cần dạy của lớp
            var classSubjects = await _classSubjectRepo.GetClassSubjectsByClassIdAsync(dto.ClassId);

            // Bước 2: Load phân công GV cho từng môn (giả sử mỗi môn có ít nhất 1 GV)
            var assignments = new List<(Guid SubjectId, Guid TeacherId)>();
            foreach (var cs in classSubjects)
            {
                var teachers = await _teacherAssignmentRepo.GetTeachersByClassSubjectAsync(cs.Id);
                if (teachers.Any())
                {
                    assignments.Add((cs.SubjectId, teachers.First().TeacherId)); // Lấy GV đầu tiên
                }
            }

            if (!assignments.Any())
            {
                return new GenerateTimetableResult { Success = false, Message = "Không có môn nào được phân công GV cho lớp này" };
            }

            // Giả sử số tiết/tuần mặc định 4 tiết/môn (có thể thêm cột WeeklyPeriods sau)
            int periodsPerSubject = 4;
            int totalPeriodsNeeded = assignments.Count * periodsPerSubject;

            // Bước 3: Tạo slot trống (Thứ 2-7, tiết 1-10)
            var availableSlots = new List<(short Day, short Period)>();
            for (short day = 1; day <= 7; day++)
                for (short period = 1; period <= dto.NumberOfPeriodsPerDay; period++)
                    availableSlots.Add((day, period));

            // Shuffle để random
            availableSlots = availableSlots.OrderBy(_ => _random.Next()).ToList();

            // Bước 4: Gán tuần tự + check conflict
            foreach (var (subjectId, teacherId) in assignments)
            {
                bool assignedSubject = false;
                for (int retry = 0; retry < dto.MaxRetries && !assignedSubject; retry++)
                {
                    foreach (var slot in availableSlots)
                    {
                        // Check conflict
                        bool classConflict = await _timetableRepo.HasConflictForClassAsync(dto.ClassId, slot.Day, slot.Period);
                        bool teacherConflict = await _timetableRepo.HasConflictForTeacherAsync(teacherId, slot.Day, slot.Period);

                        if (!classConflict && !teacherConflict)
                        {
                            var timetable = new Timetable(
                                dto.ClassId,
                                subjectId,
                                teacherId,
                                slot.Day,
                                slot.Period,
                                null, null, null, dto.SchoolYear);

                            await _timetableRepo.AddAsync(timetable);
                            assigned++;
                            assignedSubject = true;
                            break;
                        }
                    }
                }

                if (!assignedSubject)
                {
                    warnings.Add($"Không thể gán đủ tiết cho môn {subjectId} (GV {teacherId})");
                }
            }

            return new GenerateTimetableResult
            {
                Success = true,
                AssignedCount = assigned,
                Warnings = warnings,
                Message = warnings.Any() ? "Tạo tự động thành công nhưng có một số môn chưa gán đủ tiết" : "Tạo tự động thành công"
            };
        }
    }
}