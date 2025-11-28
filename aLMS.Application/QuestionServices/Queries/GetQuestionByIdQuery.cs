using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.QuestionServices.Queries
{
    public class GetQuestionByIdQuery : IRequest<QuestionDto>
    {
        public Guid Id { get; set; }
    }

    public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDto>
    {
        private readonly IQuestionRepository _repo;
        private readonly IAnswerRepository _answerRepo;
        private readonly IMapper _mapper;

        public GetQuestionByIdQueryHandler(IQuestionRepository repo, IAnswerRepository answerRepo, IMapper mapper)
        {
            _repo = repo;
            _answerRepo = answerRepo;
            _mapper = mapper;
        }

        public async Task<QuestionDto> Handle(GetQuestionByIdQuery request, CancellationToken ct)
        {
            var question = await _repo.GetQuestionByIdAsync(request.Id);
            if (question == null) return null;

            var dto = _mapper.Map<QuestionDto>(question);
            var answers = await _answerRepo.GetAnswersByQuestionIdAsync(request.Id);
            dto.Answers = _mapper.Map<List<AnswerDto>>(answers);

            return dto;
        }
    }
}