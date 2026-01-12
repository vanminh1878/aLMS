using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.TimetableServices.Queries
{
    public class GetTimetableByClassQuery : IRequest<IEnumerable<TimetableDto>>
    {
        public Guid ClassId { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class GetTimetableByClassQueryHandler : IRequestHandler<GetTimetableByClassQuery, IEnumerable<TimetableDto>>
    {
        private readonly ITimetableRepository _repo;
        private readonly IMapper _mapper;

        public GetTimetableByClassQueryHandler(ITimetableRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TimetableDto>> Handle(GetTimetableByClassQuery request, CancellationToken ct)
        {
            var timetables = await _repo.GetByClassIdAsync(request.ClassId, request.SchoolYear);
            return _mapper.Map<IEnumerable<TimetableDto>>(timetables);
        }
    }
}