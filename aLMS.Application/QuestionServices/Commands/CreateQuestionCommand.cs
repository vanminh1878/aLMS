// aLMS.Application.QuestionServices.Commands.CreateQuestion/CreateQuestionCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.QuestionEntity;
using aLMS.Domain.AnswerEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.QuestionServices.Commands.CreateQuestion
{
    public class CreateQuestionCommand : IRequest<Guid>
    {
        public CreateQuestionDto QuestionDto { get; set; } = null!;
    }

    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Guid>
    {
        private readonly IQuestionRepository _questionRepo;
        private readonly IAnswerRepository _answerRepo;
        private readonly IMapper _mapper;

        public CreateQuestionCommandHandler(IQuestionRepository questionRepo, IAnswerRepository answerRepo, IMapper mapper)
        {
            _questionRepo = questionRepo;
            _answerRepo = answerRepo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateQuestionCommand request, CancellationToken ct)
        {
            var question = _mapper.Map<Question>(request.QuestionDto);
            question.Id = Guid.NewGuid();

            foreach (var answerDto in request.QuestionDto.Answers)
            {
                var answer = _mapper.Map<Answer>(answerDto);
                answer.Id = Guid.NewGuid();
                answer.QuestionId = question.Id;
                answer.RaiseAnswerCreatedEvent();
                question.Answers.Add(answer);
            }

            question.RaiseQuestionCreatedEvent();
            await _questionRepo.AddQuestionAsync(question);

            return question.Id;
        }
    }
}