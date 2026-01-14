using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentEvaluationServices.Queries
{
    public class GetStudentEvaluationsByClassQuery : IRequest<IEnumerable<StudentEvaluationDto>>
    {
        public Guid ClassId { get; set; }
        public string? Semester { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class GetStudentEvaluationsByClassQueryHandler : IRequestHandler<GetStudentEvaluationsByClassQuery, IEnumerable<StudentEvaluationDto>>
    {
        private readonly IStudentEvaluationRepository _repo;
        private readonly IMapper _mapper;

        public GetStudentEvaluationsByClassQueryHandler(IStudentEvaluationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentEvaluationDto>> Handle(GetStudentEvaluationsByClassQuery request, CancellationToken ct)
        {
            var entities = await _repo.GetByClassIdAsync(request.ClassId, request.Semester, request.SchoolYear);
            return _mapper.Map<IEnumerable<StudentEvaluationDto>>(entities);
        }
    }
}