// aLMS.Application.ExerciseServices.Commands.UpdateExercise/UpdateExerciseCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ExerciseEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ExerciseServices.Commands.UpdateExercise
{
    public class UpdateExerciseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? ExerciseId { get; set; }
    }

    public class UpdateExerciseCommand : IRequest<UpdateExerciseResult>
    {
        public UpdateExerciseDto ExerciseDto { get; set; } = null!;
    }

    public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, UpdateExerciseResult>
    {
        private readonly IExerciseRepository _repo;
        private readonly IMapper _mapper;

        public UpdateExerciseCommandHandler(IExerciseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdateExerciseResult> Handle(UpdateExerciseCommand request, CancellationToken ct)
        {
            var exercise = _mapper.Map<Exercise>(request.ExerciseDto);

            try
            {
                exercise.RaiseExerciseUpdatedEvent();
                await _repo.UpdateExerciseAsync(exercise);
                return new UpdateExerciseResult
                {
                    Success = true,
                    Message = "Exercise updated successfully.",
                    ExerciseId = exercise.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateExerciseResult
                {
                    Success = false,
                    Message = $"Error updating exercise: {ex.Message}"
                };
            }
        }
    }
}