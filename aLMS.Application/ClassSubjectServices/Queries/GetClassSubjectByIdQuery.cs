using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.ClassSubjectServices.Queries
{
    public class GetClassSubjectByIdQuery : IRequest<ClassSubjectDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetClassSubjectByIdQueryHandler : IRequestHandler<GetClassSubjectByIdQuery, ClassSubjectDto?>
    {
        private readonly IClassSubjectRepository _repo;
        private readonly IMapper _mapper;

        public GetClassSubjectByIdQueryHandler(IClassSubjectRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ClassSubjectDto?> Handle(GetClassSubjectByIdQuery request, CancellationToken ct)
        {
            var classSubject = await _repo.GetClassSubjectByIdAsync(request.Id);
            return classSubject == null ? null : _mapper.Map<ClassSubjectDto>(classSubject);
        }
    }
}