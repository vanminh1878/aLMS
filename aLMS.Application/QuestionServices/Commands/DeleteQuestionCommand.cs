// aLMS.Application.QuestionServices.Commands.DeleteQuestion/DeleteQuestionCommand.cs
using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.QuestionServices.Commands.DeleteQuestion
{
    public class DeleteQuestionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? QuestionId { get; set; }
    }

    public class DeleteQuestionCommand : IRequest<DeleteQuestionResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, DeleteQuestionResult>
    {
        private readonly IQuestionRepository _repo;

        public DeleteQuestionCommandHandler(IQuestionRepository repo)
        {
            _repo = repo;
        }

        public async Task<DeleteQuestionResult> Handle(DeleteQuestionCommand request, CancellationToken ct)
        {
            var exists = await _repo.QuestionExistsAsync(request.Id);
            if (!exists)
                return new DeleteQuestionResult { Success = false, Message = "Not found", QuestionId = request.Id };

            try
            {
                await _repo.DeleteQuestionAsync(request.Id);
                return new DeleteQuestionResult { Success = true, Message = "Deleted", QuestionId = request.Id };
            }
            catch (Exception ex)
            {
                return new DeleteQuestionResult { Success = false, Message = ex.Message, QuestionId = request.Id };
            }
        }
    }
}