using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.NotificationServices.Queries
{
    public class GetNotificationsByUserQuery : IRequest<IEnumerable<NotificationDto>>
    {
        public Guid UserId { get; set; }
    }

    public class GetNotificationsByUserQueryHandler : IRequestHandler<GetNotificationsByUserQuery, IEnumerable<NotificationDto>>
    {
        private readonly INotificationRepository _repo;
        private readonly IMapper _mapper;

        public GetNotificationsByUserQueryHandler(INotificationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationsByUserQuery request, CancellationToken ct)
        {
            var notifications = await _repo.GetByUserIdAsync(request.UserId);
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }
    }
}