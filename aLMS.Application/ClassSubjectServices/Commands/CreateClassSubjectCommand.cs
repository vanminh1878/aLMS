using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassSubjectEntity;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.ClassSubjectServices.Commands.CreateClassSubject
{
    public class CreateClassSubjectCommand : IRequest<Guid>
    {
        public CreateClassSubjectDto ClassSubjectDto { get; set; }
    }

    public class CreateClassSubjectCommandHandler : IRequestHandler<CreateClassSubjectCommand, Guid>
    {
        private readonly IClassSubjectRepository _repo;
        private readonly IMapper _mapper;

        public CreateClassSubjectCommandHandler(IClassSubjectRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateClassSubjectCommand request, CancellationToken ct)
        {
            var classSubject = _mapper.Map<ClassSubject>(request.ClassSubjectDto);
            classSubject.RaiseCreatedEvent();
            await _repo.AddClassSubjectAsync(classSubject);
            return classSubject.Id;
        }
    }
}