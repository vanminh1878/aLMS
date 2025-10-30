using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.SubjectServices.Queries
{
    public class GetSubjectByIdQuery : IRequest<SubjectDto>
    {
        public Guid Id { get; set; }
    }

    public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, SubjectDto>
    {
        private readonly ISubjectRepository _repo;
        private readonly IMapper _mapper;

        public GetSubjectByIdQueryHandler(ISubjectRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<SubjectDto> Handle(GetSubjectByIdQuery request, CancellationToken ct)
        {
            var subject = await _repo.GetSubjectByIdAsync(request.Id);
            return _mapper.Map<SubjectDto>(subject);
        }
    }
}