using aLMS.Application.Common.Interfaces;
using MediatR;
using System;

namespace aLMS.Application.VirtualClassroomServices.Commands
{
    public class DeleteVirtualClassroomCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteVirtualClassroomCommandHandler : IRequestHandler<DeleteVirtualClassroomCommand, bool>
    {
        private readonly IVirtualClassroomRepository _repo;

        public DeleteVirtualClassroomCommandHandler(IVirtualClassroomRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteVirtualClassroomCommand request, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return false;

            existing.RaiseDeletedEvent();
            await _repo.DeleteAsync(request.Id);
            return true;
        }
    }
}