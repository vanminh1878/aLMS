// aLMS.Application.AnswerServices.Queries/GetAnswersByQuestionIdQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.AnswerServices.Queries
{
    public class GetAnswersByQuestionIdQuery : IRequest<IEnumerable<AnswerDto>>
    {
        public Guid QuestionId { get; set; }
    }

    public class GetAnswersByQuestionIdQueryHandler : IRequestHandler<GetAnswersByQuestionIdQuery, IEnumerable<AnswerDto>>
    {
        private readonly IAnswerRepository _repo;
        private readonly IMapper _mapper;

        public GetAnswersByQuestionIdQueryHandler(IAnswerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AnswerDto>> Handle(GetAnswersByQuestionIdQuery request, CancellationToken ct)
        {
            var answers = await _repo.GetAnswersByQuestionIdAsync(request.QuestionId);
            return _mapper.Map<IEnumerable<AnswerDto>>(answers);
        }
    }
}