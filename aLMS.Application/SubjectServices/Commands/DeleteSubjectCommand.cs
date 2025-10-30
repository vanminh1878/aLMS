using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.SubjectServices.Commands.DeleteSubject
{
    public class DeleteSubjectResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class DeleteSubjectCommand : IRequest<DeleteSubjectResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, DeleteSubjectResult>
    {
        private readonly ISubjectRepository _repo;

        public DeleteSubjectCommandHandler(ISubjectRepository repo) => _repo = repo;

        public async Task<DeleteSubjectResult> Handle(DeleteSubjectCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.SubjectExistsAsync(request.Id);
                if (!exists) return new DeleteSubjectResult { Success = false, Message = "Subject not found." };

                var subject = await _repo.GetSubjectByIdAsync(request.Id);
                subject.RaiseSubjectDeletedEvent();
                await _repo.DeleteSubjectAsync(request.Id);

                return new DeleteSubjectResult { Success = true, Message = "Subject deleted." };
            }
            catch (Exception ex)
            {
                return new DeleteSubjectResult { Success = false, Message = ex.Message };
            }
        }
    }
}