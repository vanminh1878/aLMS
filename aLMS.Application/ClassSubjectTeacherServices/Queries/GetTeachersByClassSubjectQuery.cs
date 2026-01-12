using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.ClassSubjectTeacherServices.Queries
{
    public class GetTeachersByClassSubjectQuery : IRequest<IEnumerable<ClassSubjectTeacherDto>>
    {
        public Guid ClassSubjectId { get; set; }
    }

    public class GetTeachersByClassSubjectQueryHandler : IRequestHandler<GetTeachersByClassSubjectQuery, IEnumerable<ClassSubjectTeacherDto>>
    {
        private readonly IClassSubjectTeacherRepository _repo;
        private readonly IMapper _mapper;

        public GetTeachersByClassSubjectQueryHandler(IClassSubjectTeacherRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassSubjectTeacherDto>> Handle(GetTeachersByClassSubjectQuery request, CancellationToken ct)
        {
            var assignments = await _repo.GetTeachersByClassSubjectAsync(request.ClassSubjectId);
            return _mapper.Map<IEnumerable<ClassSubjectTeacherDto>>(assignments);
        }
    }
}