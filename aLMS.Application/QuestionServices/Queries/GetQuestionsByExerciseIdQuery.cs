// aLMS.Application.QuestionServices.Queries/GetQuestionsByExerciseIdQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.QuestionServices.Queries
{
    public class GetQuestionsByExerciseIdQuery : IRequest<IEnumerable<QuestionDto>>
    {
        public Guid ExerciseId { get; set; }
    }

    public class GetQuestionsByExerciseIdQueryHandler : IRequestHandler<GetQuestionsByExerciseIdQuery, IEnumerable<QuestionDto>>
    {
        private readonly IQuestionRepository _repo;
        private readonly IAnswerRepository _answerRepo;
        private readonly IMapper _mapper;

        public GetQuestionsByExerciseIdQueryHandler(IQuestionRepository repo, IAnswerRepository answerRepo, IMapper mapper)
        {
            _repo = repo;
            _answerRepo = answerRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuestionDto>> Handle(GetQuestionsByExerciseIdQuery request, CancellationToken ct)
        {
            var questions = await _repo.GetQuestionsByExerciseIdAsync(request.ExerciseId);
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