using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.NotificationServices.Queries
{
    public class GetNotificationByIdQuery : IRequest<NotificationDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationDto?>
    {
        private readonly INotificationRepository _repo;
        private readonly IMapper _mapper;

        public GetNotificationByIdQueryHandler(INotificationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<NotificationDto?> Handle(GetNotificationByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            return entity == null ? null : _mapper.Map<NotificationDto>(entity);
        }
    }
}