using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.ClassSubjectServices.Queries
{
    public class GetClassSubjectsByClassIdQuery : IRequest<IEnumerable<ClassSubjectDto>>
    {
        public Guid ClassId { get; set; }
    }

    public class GetClassSubjectsByClassIdQueryHandler : IRequestHandler<GetClassSubjectsByClassIdQuery, IEnumerable<ClassSubjectDto>>
    {
        private readonly IClassSubjectRepository _repo;
        private readonly IMapper _mapper;

        public GetClassSubjectsByClassIdQueryHandler(IClassSubjectRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassSubjectDto>> Handle(GetClassSubjectsByClassIdQuery request, CancellationToken ct)
        {
            var classSubjects = await _repo.GetClassSubjectsByClassIdAsync(request.ClassId);
            return _mapper.Map<IEnumerable<ClassSubjectDto>>(classSubjects);
        }
    }
}