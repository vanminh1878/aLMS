using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.SubjectServices.Queries
{
    public class GetAllSubjectsQuery : IRequest<IEnumerable<SubjectDto>> { }

    public class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, IEnumerable<SubjectDto>>
    {
        private readonly ISubjectRepository _repo;
        private readonly IMapper _mapper;

        public GetAllSubjectsQueryHandler(ISubjectRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubjectDto>> Handle(GetAllSubjectsQuery request, CancellationToken ct)
        {
            var subjects = await _repo.GetAllSubjectsAsync();
            return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }
    }
}