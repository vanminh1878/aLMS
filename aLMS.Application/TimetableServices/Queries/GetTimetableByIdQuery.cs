using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.TimetableServices.Queries
{
    public class GetTimetableByIdQuery : IRequest<TimetableDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetTimetableByIdQueryHandler : IRequestHandler<GetTimetableByIdQuery, TimetableDto?>
    {
        private readonly ITimetableRepository _repo;
        private readonly IMapper _mapper;

        public GetTimetableByIdQueryHandler(ITimetableRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TimetableDto?> Handle(GetTimetableByIdQuery request, CancellationToken ct)
        {
            var timetable = await _repo.GetByIdAsync(request.Id);
            return timetable == null ? null : _mapper.Map<TimetableDto>(timetable);
        }
    }
}