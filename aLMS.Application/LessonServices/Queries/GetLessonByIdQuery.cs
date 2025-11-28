using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.LessonServices.Queries
{
    public class GetLessonByIdQuery : IRequest<LessonDto>
    {
        public Guid Id { get; set; }
    }

    public class GetLessonByIdQueryHandler : IRequestHandler<GetLessonByIdQuery, LessonDto>
    {
        private readonly ILessonRepository _repo;
        private readonly IMapper _mapper;

        public GetLessonByIdQueryHandler(ILessonRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<LessonDto> Handle(GetLessonByIdQuery request, CancellationToken ct)
        {
            var lesson = await _repo.GetLessonByIdAsync(request.Id);
            return lesson == null ? null : _mapper.Map<LessonDto>(lesson);
        }
    }
}