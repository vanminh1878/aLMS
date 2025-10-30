using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.TopicEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TopicServices.Commands.UpdateTopic
{
    public class UpdateTopicResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? TopicId { get; set; }
    }

    public class UpdateTopicCommand : IRequest<UpdateTopicResult>
    {
        public UpdateTopicDto TopicDto { get; set; }
    }

    public class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand, UpdateTopicResult>
    {
        private readonly ITopicRepository _repo;
        private readonly IMapper _mapper;

        public UpdateTopicCommandHandler(ITopicRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdateTopicResult> Handle(UpdateTopicCommand request, CancellationToken ct)
        {
            var topic = _mapper.Map<Topic>(request.TopicDto);
            try
            {
                topic.RaiseTopicUpdatedEvent();
                await _repo.UpdateTopicAsync(topic);
                return new UpdateTopicResult { Success = true, Message = "Topic updated.", TopicId = topic.Id };
            }
            catch (Exception ex)
            {
                return new UpdateTopicResult { Success = false, Message = ex.Message };
            }
        }
    }
}