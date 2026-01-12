using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.TimetableEntity;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TimetableServices.Commands
{
    public class CreateTimetableCommand : IRequest<Guid>
    {
        public CreateTimetableDto Dto { get; set; }
    }

    public class CreateTimetableCommandHandler : IRequestHandler<CreateTimetableCommand, Guid>
    {
        private readonly ITimetableRepository _repo;

        public CreateTimetableCommandHandler(ITimetableRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateTimetableCommand request, CancellationToken ct)
        {
            var dto = request.Dto;

            // Check conflict
            bool classConflict = await _repo.HasConflictForClassAsync(dto.ClassId, dto.DayOfWeek, dto.PeriodNumber);
            bool teacherConflict = await _repo.HasConflictForTeacherAsync(dto.TeacherId, dto.DayOfWeek, dto.PeriodNumber);

            if (classConflict || teacherConflict)
            {
                throw new InvalidOperationException("Lớp hoặc GV đã có tiết trùng");
            }

            var timetable = new Timetable(
                dto.ClassId,
                dto.SubjectId,
                dto.TeacherId,
                dto.DayOfWeek,
                dto.PeriodNumber,
                dto.StartTime,
                dto.EndTime,
                dto.Room,
                dto.SchoolYear);

            await _repo.AddAsync(timetable);
            return timetable.Id;
        }
    }
}