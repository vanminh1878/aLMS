// aLMS.Application.ExerciseServices.Commands.CreateExercise/CreateExerciseCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ExerciseEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ExerciseServices.Commands.CreateExercise
{
    public class CreateExerciseCommand : IRequest<Guid>
    {
        public CreateExerciseDto ExerciseDto { get; set; } = null!;
    }

    public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Guid>
    {
        private readonly IExerciseRepository _repo;
        private readonly IMapper _mapper;

        public CreateExerciseCommandHandler(IExerciseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateExerciseCommand request, CancellationToken ct)
        {
            var exercise = _mapper.Map<Exercise>(request.ExerciseDto);
            exercise.Id = Guid.NewGuid();
            exercise.RaiseExerciseCreatedEvent();
            await _repo.AddExerciseAsync(exercise);
            return exercise.Id;
        }
    }
}