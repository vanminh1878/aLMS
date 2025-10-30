using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.SubjectEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.SubjectServices.Commands.CreateSubject
{
    public class CreateSubjectCommand : IRequest<Guid>
    {
        public CreateSubjectDto SubjectDto { get; set; }
    }

    public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, Guid>
    {
        private readonly ISubjectRepository _repo;
        private readonly IMapper _mapper;

        public CreateSubjectCommandHandler(ISubjectRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateSubjectCommand request, CancellationToken ct)
        {
            var subject = _mapper.Map<Subject>(request.SubjectDto);
            subject.RaiseSubjectCreatedEvent();
            await _repo.AddSubjectAsync(subject);
            return subject.Id;
        }
    }
}