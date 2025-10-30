using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.GradeServices.Queries
{
    public class GetAllGradesQuery : IRequest<IEnumerable<GradeDto>> { }

    public class GetAllGradesQueryHandler : IRequestHandler<GetAllGradesQuery, IEnumerable<GradeDto>>
    {
        private readonly IGradeRepository _repo;
        private readonly IMapper _mapper;

        public GetAllGradesQueryHandler(IGradeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GradeDto>> Handle(GetAllGradesQuery request, CancellationToken ct)
        {
            var grades = await _repo.GetAllGradesAsync();
            return _mapper.Map<IEnumerable<GradeDto>>(grades);
        }
    }
}