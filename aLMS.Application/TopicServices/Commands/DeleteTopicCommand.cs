using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TopicServices.Commands.DeleteTopic
{
    public class DeleteTopicResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class DeleteTopicCommand : IRequest<DeleteTopicResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, DeleteTopicResult>
    {
        private readonly ITopicRepository _repo;

        public DeleteTopicCommandHandler(ITopicRepository repo) => _repo = repo;

        public async Task<DeleteTopicResult> Handle(DeleteTopicCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.TopicExistsAsync(request.Id);
                if (!exists) return new DeleteTopicResult { Success = false, Message = "Topic not found." };

                var topic = await _repo.GetTopicByIdAsync(request.Id);
                topic.RaiseTopicDeletedEvent();
                await _repo.DeleteTopicAsync(request.Id);

                return new DeleteTopicResult { Success = true, Message = "Topic deleted." };
            }
            catch (Exception ex)
            {
                return new DeleteTopicResult { Success = false, Message = ex.Message };
            }
        }
    }
}