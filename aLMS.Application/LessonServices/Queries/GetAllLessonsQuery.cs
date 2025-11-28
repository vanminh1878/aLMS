using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;

namespace aLMS.Application.LessonServices.Queries
{
    public class GetAllLessonsQuery : IRequest<IEnumerable<LessonDto>> { }

    public class GetAllLessonsQueryHandler : IRequestHandler<GetAllLessonsQuery, IEnumerable<LessonDto>>
    {
        private readonly ILessonRepository _repo;
        private readonly IMapper _mapper;

        public GetAllLessonsQueryHandler(ILessonRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LessonDto>> Handle(GetAllLessonsQuery request, CancellationToken ct)
        {
            var lessons = await _repo.GetAllLessonsAsync();
            return _mapper.Map<IEnumerable<LessonDto>>(lessons);
        }
    }
}