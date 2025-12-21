using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentExerciseEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentExerciseServices.Commands.StartExercise
{
    public class StartExerciseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? StudentExerciseId { get; set; }
    }

    public class StartExerciseCommand : IRequest<StartExerciseResult>
    {
        public Guid ExerciseId { get; set; }
        public Guid StudentId { get; set; }
    }

    public class StartExerciseCommandHandler : IRequestHandler<StartExerciseCommand, StartExerciseResult>
    {
        private readonly IStudentExerciseRepository _seRepo;
        private readonly IExerciseRepository _exRepo;

        public StartExerciseCommandHandler(IStudentExerciseRepository seRepo, IExerciseRepository exRepo)
        {
            _seRepo = seRepo;
            _exRepo = exRepo;
        }

        public async Task<StartExerciseResult> Handle(StartExerciseCommand request, CancellationToken ct)
        {
            try
            {
                var active = await _seRepo.GetActiveByStudentAndExerciseAsync(request.StudentId, request.ExerciseId);
                if (active != null)
                {
                    return new StartExerciseResult
                    {
                        Success = true,
                        Message = "Active session found.",
                        StudentExerciseId = active.Id
                    };
                }

                var exercise = await _exRepo.GetExerciseByIdAsync(request.ExerciseId);
                if (exercise == null)
                {
                    return new StartExerciseResult
                    {
                        Success = false,
                        Message = "Exercise not found."
                    };
                }

                var se = new StudentExercise
                {
                    Id = Guid.NewGuid(),
                    StudentId = request.StudentId,
                    ExerciseId = request.ExerciseId,
                    StartTime = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                    AttemptNumber = 1
                };

                se.RaiseStartedEvent();
                await _seRepo.AddAsync(se);

                return new StartExerciseResult
                {
                    Success = true,
                    Message = "Exercise started successfully.",
                    StudentExerciseId = se.Id
                };
            }
            catch (Exception ex)
            {
                return new StartExerciseResult
                {
                    Success = false,
                    Message = $"Error starting exercise: {ex.Message}"
                };
            }
        }
    }
}