using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.SubjectServices.Queries
{
    public class GetSubjectsByClassIdQuery : IRequest<IEnumerable<SubjectDto>>
    {
        public Guid ClassId { get; set; }
    }

    public class GetSubjectsByClassIdQueryHandler : IRequestHandler<GetSubjectsByClassIdQuery, IEnumerable<SubjectDto>>
    {
        private readonly ISubjectRepository _repo;
        private readonly IMapper _mapper;

        public GetSubjectsByClassIdQueryHandler(ISubjectRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubjectDto>> Handle(GetSubjectsByClassIdQuery request, CancellationToken ct)
        {
            var subjects = await _repo.GetSubjectsByClassIdAsync(request.ClassId);
            return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }
    }
}