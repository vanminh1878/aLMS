using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.NotificationServices.Queries
{
    public class GetNotificationsByClassQuery : IRequest<IEnumerable<NotificationDto>>
    {
        public Guid ClassId { get; set; }
    }

    public class GetNotificationsByClassQueryHandler : IRequestHandler<GetNotificationsByClassQuery, IEnumerable<NotificationDto>>
    {
        private readonly INotificationRepository _repo;
        private readonly IMapper _mapper;

        public GetNotificationsByClassQueryHandler(INotificationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationsByClassQuery request, CancellationToken ct)
        {
            var notifications = await _repo.GetByClassIdAsync(request.ClassId);
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }
    }
}