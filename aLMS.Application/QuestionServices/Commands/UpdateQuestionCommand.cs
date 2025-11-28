using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.QuestionEntity;
using aLMS.Domain.AnswerEntity;
using AutoMapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.QuestionServices.Commands.UpdateQuestion
{
    public class UpdateQuestionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? QuestionId { get; set; }
    }

    public class UpdateQuestionCommand : IRequest<UpdateQuestionResult>
    {
        public UpdateQuestionDto QuestionDto { get; set; } = null!;
    }

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, UpdateQuestionResult>
    {
        private readonly IQuestionRepository _questionRepo;
        private readonly IAnswerRepository _answerRepo;
        private readonly IMapper _mapper;

        public UpdateQuestionCommandHandler(IQuestionRepository questionRepo, IAnswerRepository answerRepo, IMapper mapper)
        {
            _questionRepo = questionRepo;
            _answerRepo = answerRepo;
            _mapper = mapper;
        }

        public async Task<UpdateQuestionResult> Handle(UpdateQuestionCommand request, CancellationToken ct)
        {
            try
            {
                var question = _mapper.Map<Question>(request.QuestionDto);

                // Xử lý Answers: xóa cũ, thêm mới, cập nhật
                var existingAnswers = await _answerRepo.GetAnswersByQuestionIdAsync(question.Id);
                foreach (var ans in existingAnswers)
                {
                    if (!request.QuestionDto.Answers.Any(a => a.Id == ans.Id))
                        await _answerRepo.DeleteAnswerAsync(ans.Id);
                }

                foreach (var dto in request.QuestionDto.Answers)
                {
                    if (dto.Id == Guid.Empty)
                    {
                        var newAns = _mapper.Map<Answer>(dto);
                        newAns.Id = Guid.NewGuid();
                        newAns.QuestionId = question.Id;
                        newAns.RaiseAnswerCreatedEvent();
                        question.Answers.Add(newAns);
                    }
                    else
                    {
                        var existing = question.Answers.FirstOrDefault(a => a.Id == dto.Id);
                        if (existing != null)
                        {
                            _mapper.Map(dto, existing);
                            existing.RaiseAnswerUpdatedEvent();
                        }
                    }
                }

                question.RaiseQuestionUpdatedEvent();
                await _questionRepo.UpdateQuestionAsync(question);

                return new UpdateQuestionResult
                {
                    Success = true,
                    Message = "Question updated.",
                    QuestionId = question.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateQuestionResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}