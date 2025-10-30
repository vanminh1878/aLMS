// aLMS.Application.LessonServices.Commands.UpdateLesson/UpdateLessonCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.LessonEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.LessonServices.Commands.UpdateLesson
{
    public class UpdateLessonResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? LessonId { get; set; }
    }

    public class UpdateLessonCommand : IRequest<UpdateLessonResult>
    {
        public UpdateLessonDto LessonDto { get; set; } = null!;
    }

    public class UpdateLessonCommandHandler : IRequestHandler<UpdateLessonCommand, UpdateLessonResult>
    {
        private readonly ILessonRepository _repo;
        private readonly IMapper _mapper;

        public UpdateLessonCommandHandler(ILessonRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdateLessonResult> Handle(UpdateLessonCommand request, CancellationToken ct)
        {
            var lesson = _mapper.Map<Lesson>(request.LessonDto);

            try
            {
                lesson.RaiseLessonUpdatedEvent();
                await _repo.UpdateLessonAsync(lesson);
                return new UpdateLessonResult
                {
                    Success = true,
                    Message = "Lesson updated successfully.",
                    LessonId = lesson.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateLessonResult
                {
                    Success = false,
                    Message = $"Error updating lesson: {ex.Message}"
                };
            }
        }
    }
}