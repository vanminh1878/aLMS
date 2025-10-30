using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TopicServices.Queries
{
    public class GetTopicByIdQuery : IRequest<TopicDto>
    {
        public Guid Id { get; set; }
    }

    public class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, TopicDto>
    {
        private readonly ITopicRepository _repo;
        private readonly IMapper _mapper;

        public GetTopicByIdQueryHandler(ITopicRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TopicDto> Handle(GetTopicByIdQuery request, CancellationToken ct)
        {
            var topic = await _repo.GetTopicByIdAsync(request.Id);
            return _mapper.Map<TopicDto>(topic);
        }
    }
}