using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.ClassSubjectTeacherServices.Queries
{
    public class GetClassSubjectsByTeacherQuery : IRequest<IEnumerable<ClassSubjectDto>>
    {
        public Guid TeacherId { get; set; }
    }

    public class GetClassSubjectsByTeacherQueryHandler : IRequestHandler<GetClassSubjectsByTeacherQuery, IEnumerable<ClassSubjectDto>>
    {
        private readonly IClassSubjectTeacherRepository _repo;
        private readonly IMapper _mapper;

        public GetClassSubjectsByTeacherQueryHandler(IClassSubjectTeacherRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassSubjectDto>> Handle(GetClassSubjectsByTeacherQuery request, CancellationToken ct)
        {
            var assignments = await _repo.GetClassSubjectsByTeacherAsync(request.TeacherId);
            return _mapper.Map<IEnumerable<ClassSubjectDto>>(assignments);
        }
    }
}