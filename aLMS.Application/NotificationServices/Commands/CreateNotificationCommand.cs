using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.NotificationEntity;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.NotificationServices.Commands
{
    public class CreateNotificationCommand : IRequest<Guid>
    {
        public CreateNotificationDto Dto { get; set; }
    }

    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
    {
        private readonly INotificationRepository _repo;
        private readonly IMapper _mapper;

        public CreateNotificationCommandHandler(INotificationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateNotificationCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            var entity = new Notification(
                title: dto.Title,
                content: dto.Content,
                targetType: dto.TargetType,
                targetId: dto.TargetId,
                createdBy: request.Dto.CreatedBy,               // ← truyền thẳng vào constructor
                schoolId: dto.SchoolId
            );

            entity.RaiseCreatedEvent();
            await _repo.AddAsync(entity);

            return entity.Id;
        }
    }
}