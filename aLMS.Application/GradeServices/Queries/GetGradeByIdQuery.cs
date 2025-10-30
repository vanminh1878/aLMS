using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.GradeServices.Queries
{
    public class GetGradeByIdQuery : IRequest<GradeDto>
    {
        public Guid Id { get; set; }
    }

    public class GetGradeByIdQueryHandler : IRequestHandler<GetGradeByIdQuery, GradeDto>
    {
        private readonly IGradeRepository _repo;
        private readonly IMapper _mapper;

        public GetGradeByIdQueryHandler(IGradeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<GradeDto> Handle(GetGradeByIdQuery request, CancellationToken ct)
        {
            var grade = await _repo.GetGradeByIdAsync(request.Id);
            return _mapper.Map<GradeDto>(grade);
        }
    }
}