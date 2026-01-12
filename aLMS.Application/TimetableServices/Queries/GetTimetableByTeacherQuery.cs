using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.TimetableServices.Queries
{
    public class GetTimetableByTeacherQuery : IRequest<IEnumerable<TimetableDto>>
    {
        public Guid TeacherId { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class GetTimetableByTeacherQueryHandler : IRequestHandler<GetTimetableByTeacherQuery, IEnumerable<TimetableDto>>
    {
        private readonly ITimetableRepository _repo;
        private readonly IMapper _mapper;

        public GetTimetableByTeacherQueryHandler(ITimetableRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TimetableDto>> Handle(GetTimetableByTeacherQuery request, CancellationToken ct)
        {
            var timetables = await _repo.GetByTeacherIdAsync(request.TeacherId, request.SchoolYear);
            return _mapper.Map<IEnumerable<TimetableDto>>(timetables);
        }
    }
}