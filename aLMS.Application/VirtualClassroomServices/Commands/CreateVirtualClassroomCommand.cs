using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.VirtualClassroomEntity;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.VirtualClassroomServices.Commands
{
    public class CreateVirtualClassroomCommand : IRequest<Guid>
    {
        public CreateVirtualClassroomDto Dto { get; set; }
    }

    public class CreateVirtualClassroomCommandHandler : IRequestHandler<CreateVirtualClassroomCommand, Guid>
    {
        private readonly IVirtualClassroomRepository _repo;
        private readonly IMapper _mapper;

        public CreateVirtualClassroomCommandHandler(IVirtualClassroomRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateVirtualClassroomCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            var entity = new VirtualClassroom(
                classId: dto.ClassId,
                title: dto.Title,
                meetingUrl: dto.MeetingUrl,
                startTime: dto.StartTime,
                endTime: dto.EndTime,
                createdBy: dto.CreatedBy,           
                isRecurring: dto.IsRecurring,
                subjectId: dto.SubjectId,
                timetableId: dto.TimetableId,
                dayOfWeek: dto.DayOfWeek,
                periodNumber: dto.PeriodNumber,
                meetingId: dto.MeetingId,
                password: dto.Password
            );
            entity.RaiseCreatedEvent();
            await _repo.AddAsync(entity);
            return entity.Id;
        }
    }
}