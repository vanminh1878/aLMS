using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TimetableServices.Commands
{
    public class DeleteTimetableCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteTimetableCommandHandler : IRequestHandler<DeleteTimetableCommand, bool>
    {
        private readonly ITimetableRepository _repo;

        public DeleteTimetableCommandHandler(ITimetableRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteTimetableCommand request, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return false;

            await _repo.DeleteAsync(request.Id);
            return true;
        }
    }
}