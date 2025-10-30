using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.SubjectEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.SubjectServices.Commands.UpdateSubject
{
    public class UpdateSubjectResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? SubjectId { get; set; }
    }

    public class UpdateSubjectCommand : IRequest<UpdateSubjectResult>
    {
        public UpdateSubjectDto SubjectDto { get; set; }
    }

    public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, UpdateSubjectResult>
    {
        private readonly ISubjectRepository _repo;
        private readonly IMapper _mapper;

        public UpdateSubjectCommandHandler(ISubjectRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdateSubjectResult> Handle(UpdateSubjectCommand request, CancellationToken ct)
        {
            var subject = _mapper.Map<Subject>(request.SubjectDto);
            try
            {
                subject.RaiseSubjectUpdatedEvent();
                await _repo.UpdateSubjectAsync(subject);
                return new UpdateSubjectResult { Success = true, Message = "Subject updated.", SubjectId = subject.Id };
            }
            catch (Exception ex)
            {
                return new UpdateSubjectResult { Success = false, Message = ex.Message };
            }
        }
    }
}