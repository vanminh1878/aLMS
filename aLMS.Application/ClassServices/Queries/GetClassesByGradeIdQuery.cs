using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Queries
{
    public class GetClassesByGradeIdQuery : IRequest<IEnumerable<ClassDto>>
    {
        public Guid GradeId { get; set; }
    }

    public class GetClassesByGradeIdQueryHandler : IRequestHandler<GetClassesByGradeIdQuery, IEnumerable<ClassDto>>
    {
        private readonly IClassRepository _repo;
        private readonly IMapper _mapper;

        public GetClassesByGradeIdQueryHandler(IClassRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassDto>> Handle(GetClassesByGradeIdQuery request, CancellationToken ct)
        {
            var classes = await _repo.GetClassesByGradeIdAsync(request.GradeId);
            return _mapper.Map<IEnumerable<ClassDto>>(classes);
        }
    }
}