using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.TopicEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TopicServices.Commands.CreateTopic
{
    public class CreateTopicCommand : IRequest<Guid>
    {
        public CreateTopicDto TopicDto { get; set; }
    }

    public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Guid>
    {
        private readonly ITopicRepository _repo;
        private readonly IMapper _mapper;

        public CreateTopicCommandHandler(ITopicRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTopicCommand request, CancellationToken ct)
        {
            var topic = _mapper.Map<Topic>(request.TopicDto);
            topic.RaiseTopicCreatedEvent();
            await _repo.AddTopicAsync(topic);
            return topic.Id;
        }
    }
}