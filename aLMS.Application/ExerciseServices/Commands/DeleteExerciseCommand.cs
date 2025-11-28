using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ExerciseServices.Commands.DeleteExercise
{
    public class DeleteExerciseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? ExerciseId { get; set; }
    }

    public class DeleteExerciseCommand : IRequest<DeleteExerciseResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, DeleteExerciseResult>
    {
        private readonly IExerciseRepository _repo;

        public DeleteExerciseCommandHandler(IExerciseRepository repo)
        {
            _repo = repo;
        }

        public async Task<DeleteExerciseResult> Handle(DeleteExerciseCommand request, CancellationToken ct)
        {
            var exists = await _repo.ExerciseExistsAsync(request.Id);
            if (!exists)
            {
                return new DeleteExerciseResult
                {
                    Success = false,
                    Message = "Exercise not found.",
                    ExerciseId = request.Id
                };
            }

            try
            {
                await _repo.DeleteExerciseAsync(request.Id);
                return new DeleteExerciseResult
                {
                    Success = true,
                    Message = "Exercise deleted successfully.",
                    ExerciseId = request.Id
                };
            }
            catch (Exception ex)
            {
                return new DeleteExerciseResult
                {
                    Success = false,
                    Message = $"Error deleting exercise: {ex.Message}",
                    ExerciseId = request.Id
                };
            }
        }
    }
}