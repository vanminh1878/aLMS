using aLMS.Application.Common.Interfaces;
using MediatR;
using System;

namespace aLMS.Application.NotificationServices.Commands
{
    public class DeleteNotificationCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, bool>
    {
        private readonly INotificationRepository _repo;

        public DeleteNotificationCommandHandler(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return false;

            await _repo.DeleteAsync(request.Id);
            return true;
        }
    }
}