using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.TimetableServices.Queries
{
    public class GetTimetableByStudentQuery : IRequest<IEnumerable<TimetableDto>>
    {
        public Guid StudentId { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class GetTimetableByStudentQueryHandler : IRequestHandler<GetTimetableByStudentQuery, IEnumerable<TimetableDto>>
    {
        private readonly ITimetableRepository _repo;
        private readonly IMapper _mapper;

        public GetTimetableByStudentQueryHandler(ITimetableRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TimetableDto>> Handle(GetTimetableByStudentQuery request, CancellationToken ct)
        {
            var timetables = await _repo.GetByStudentIdAsync(request.StudentId, request.SchoolYear);
            return _mapper.Map<IEnumerable<TimetableDto>>(timetables);
        }
    }
}