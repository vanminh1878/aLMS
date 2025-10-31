// aLMS.Application.QuestionServices.Queries/GetAllQuestionsQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.QuestionServices.Queries
{
    public class GetAllQuestionsQuery : IRequest<IEnumerable<QuestionDto>> { }

    public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, IEnumerable<QuestionDto>>
    {
        private readonly IQuestionRepository _repo;
        private readonly IAnswerRepository _answerRepo;
        private readonly IMapper _mapper;

        public GetAllQuestionsQueryHandler(IQuestionRepository repo, IAnswerRepository answerRepo, IMapper mapper)
        {
            _repo = repo;
            _answerRepo = answerRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuestionDto>> Handle(GetAllQuestionsQuery request, CancellationToken ct)
        {
            var questions = await _repo.GetAllQuestionsAsync();
            var dtos = _mapper.Map<IEnumerable<QuestionDto>>(questions);

            foreach (var dto in dtos)
            {
                var answers = await _answerRepo.GetAnswersByQuestionIdAsync(dto.Id);
                dto.Answers = _mapper.Map<List<AnswerDto>>(answers);
            }

            return dtos;
        }
    }
}