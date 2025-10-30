// GetLessonsByTopicIdQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.LessonServices.Queries
{
    public class GetLessonsByTopicIdQuery : IRequest<IEnumerable<LessonDto>>
    {
        public Guid TopicId { get; set; }
    }

    public class GetLessonsByTopicIdQueryHandler : IRequestHandler<GetLessonsByTopicIdQuery, IEnumerable<LessonDto>>
    {
        private readonly ILessonRepository _repo;
        private readonly IMapper _mapper;

        public GetLessonsByTopicIdQueryHandler(ILessonRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LessonDto>> Handle(GetLessonsByTopicIdQuery request, CancellationToken ct)
        {
            var lessons = await _repo.GetLessonsByTopicIdAsync(request.TopicId);
            return _mapper.Map<IEnumerable<LessonDto>>(lessons);
        }
    }
}