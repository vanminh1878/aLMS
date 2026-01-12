using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.TimetableEntity;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TimetableServices.Commands
{
    public class UpdateTimetableCommand : IRequest<bool>
    {
        public UpdateTimetableDto Dto { get; set; }
    }

    public class UpdateTimetableCommandHandler : IRequestHandler<UpdateTimetableCommand, bool>
    {
        private readonly ITimetableRepository _repo;

        public UpdateTimetableCommandHandler(ITimetableRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(UpdateTimetableCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing == null)
                return false;

            // Check conflict cho slot mới
            bool classConflict = await _repo.HasConflictForClassAsync(existing.ClassId, dto.DayOfWeek, dto.PeriodNumber);
            bool teacherConflict = await _repo.HasConflictForTeacherAsync(existing.TeacherId, dto.DayOfWeek, dto.PeriodNumber);

            if (classConflict || teacherConflict)
            {
                throw new InvalidOperationException("Lớp hoặc GV đã có tiết trùng ở vị trí mới");
            }

            existing.Update(new Timetable(existing.ClassId, existing.SubjectId, existing.TeacherId,
                dto.DayOfWeek, dto.PeriodNumber, dto.StartTime, dto.EndTime, dto.Room, dto.SchoolYear));

            await _repo.UpdateAsync(existing);
            return true;
        }
    }
}