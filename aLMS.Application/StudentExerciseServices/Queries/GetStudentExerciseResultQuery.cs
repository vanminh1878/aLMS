using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentExerciseServices.Queries
{
    public class GetStudentExerciseResultQuery : IRequest<StudentExerciseDto>
    {
        public Guid Id { get; set; }
    }

    public class GetStudentExerciseResultQueryHandler : IRequestHandler<GetStudentExerciseResultQuery, StudentExerciseDto>
    {
        private readonly IStudentExerciseRepository _seRepo;
        private readonly IStudentAnswerRepository _saRepo;
        private readonly IMapper _mapper;

        public GetStudentExerciseResultQueryHandler(
            IStudentExerciseRepository seRepo,
            IStudentAnswerRepository saRepo,
            IMapper mapper)
        {
            _seRepo = seRepo;
            _saRepo = saRepo;
            _mapper = mapper;
        }

        public async Task<StudentExerciseDto> Handle(GetStudentExerciseResultQuery request, CancellationToken ct)
        {
            var se = await _seRepo.GetByIdAsync(request.Id);
            if (se == null) return null;

            var answers = await _saRepo.GetByStudentExerciseIdAsync(request.Id);
            var dto = _mapper.Map<StudentExerciseDto>(se);
            dto.Answers = _mapper.Map<List<StudentAnswerDto>>(answers);
            dto.ExerciseTitle = se.Exercise.Title;
            return dto;
        }
    }
}