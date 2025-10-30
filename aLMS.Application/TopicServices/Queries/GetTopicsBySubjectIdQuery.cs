using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TopicServices.Queries
{
    public class GetTopicsBySubjectIdQuery : IRequest<IEnumerable<TopicDto>>
    {
        public Guid SubjectId { get; set; }
    }

    public class GetTopicsBySubjectIdQueryHandler : IRequestHandler<GetTopicsBySubjectIdQuery, IEnumerable<TopicDto>>
    {
        private readonly ITopicRepository _repo;
        private readonly IMapper _mapper;

        public GetTopicsBySubjectIdQueryHandler(ITopicRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TopicDto>> Handle(GetTopicsBySubjectIdQuery request, CancellationToken ct)
        {
            var topics = await _repo.GetTopicsBySubjectIdAsync(request.SubjectId);
            return _mapper.Map<IEnumerable<TopicDto>>(topics);
        }
    }
}