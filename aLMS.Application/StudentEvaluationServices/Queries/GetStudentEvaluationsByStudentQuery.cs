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
    public class GetStudentEvaluationsByStudentQuery : IRequest<IEnumerable<StudentEvaluationDto>>
    {
        public Guid StudentId { get; set; }
        public string? Semester { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class GetStudentEvaluationsByStudentQueryHandler : IRequestHandler<GetStudentEvaluationsByStudentQuery, IEnumerable<StudentEvaluationDto>>
    {
        private readonly IStudentEvaluationRepository _repo;
        private readonly IMapper _mapper;

        public GetStudentEvaluationsByStudentQueryHandler(IStudentEvaluationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentEvaluationDto>> Handle(GetStudentEvaluationsByStudentQuery request, CancellationToken ct)
        {
            var entities = await _repo.GetByStudentIdAsync(request.StudentId, request.Semester, request.SchoolYear);
            return _mapper.Map<IEnumerable<StudentEvaluationDto>>(entities);
        }
    }
}