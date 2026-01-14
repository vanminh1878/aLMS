using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.NotificationServices.Commands
{
    public class UpdateNotificationCommand : IRequest<bool>
    {
        public UpdateNotificationDto Dto { get; set; }
    }

    public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, bool>
    {
        private readonly INotificationRepository _repo;
        private readonly IMapper _mapper;

        public UpdateNotificationCommandHandler(INotificationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateNotificationCommand request, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(request.Dto.Id);
            if (existing == null)
                return false;

            _mapper.Map(request.Dto, existing);
            existing.RaiseUpdatedEvent();

            await _repo.UpdateAsync(existing);
            return true;
        }
    }
}