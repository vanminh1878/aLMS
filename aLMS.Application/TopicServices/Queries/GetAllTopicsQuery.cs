using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TopicServices.Queries
{
    public class GetAllTopicsQuery : IRequest<IEnumerable<TopicDto>> { }

    public class GetAllTopicsQueryHandler : IRequestHandler<GetAllTopicsQuery, IEnumerable<TopicDto>>
    {
        private readonly ITopicRepository _repo;
        private readonly IMapper _mapper;

        public GetAllTopicsQueryHandler(ITopicRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TopicDto>> Handle(GetAllTopicsQuery request, CancellationToken ct)
        {
            var topics = await _repo.GetAllTopicsAsync();
            return _mapper.Map<IEnumerable<TopicDto>>(topics);
        }
    }
}