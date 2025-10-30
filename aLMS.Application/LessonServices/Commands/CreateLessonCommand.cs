using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.LessonEntity;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.LessonServices.Commands.CreateLesson
{
    public class CreateLessonCommand : IRequest<Guid>
    {
        public CreateLessonDto LessonDto { get; set; }
    }

    public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, Guid>
    {
        private readonly ILessonRepository _repo;
        private readonly IMapper _mapper;

        public CreateLessonCommandHandler(ILessonRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateLessonCommand request, CancellationToken ct)
        {
            var lesson = _mapper.Map<Lesson>(request.LessonDto);
            lesson.Id = Guid.NewGuid();
            lesson.RaiseLessonCreatedEvent();
            await _repo.AddLessonAsync(lesson);
            return lesson.Id;
        }
    }
}