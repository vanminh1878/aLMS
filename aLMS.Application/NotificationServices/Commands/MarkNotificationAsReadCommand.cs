using aLMS.Application.Common.Interfaces;
using MediatR;
using System;

namespace aLMS.Application.NotificationServices.Commands
{
    public class MarkNotificationAsReadCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, bool>
    {
        private readonly INotificationRepository _repo;

        public MarkNotificationAsReadCommandHandler(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(MarkNotificationAsReadCommand request, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return false;

            existing.MarkAsRead();
            await _repo.UpdateAsync(existing);
            return true;
        }
    }
}