// aLMS.Application.StudentExerciseServices.Commands.SubmitExercise/SubmitExerciseCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentAnswerEntity;
using aLMS.Domain.StudentExerciseEntity;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentExerciseServices.Commands.SubmitExercise
{
    public class SubmitExerciseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? StudentExerciseId { get; set; }
        public decimal? Score { get; set; }
        public List<StudentAnswerDto> Answers { get; set; } = new();
    }

    public class SubmitExerciseCommand : IRequest<SubmitExerciseResult>
    {
        public Guid StudentExerciseId { get; set; }
        public List<SubmitAnswerDto> Answers { get; set; } = new();
    }

    public class SubmitExerciseCommandHandler : IRequestHandler<SubmitExerciseCommand, SubmitExerciseResult>
    {
        private readonly IStudentExerciseRepository _seRepo;
        private readonly IStudentAnswerRepository _saRepo;
        private readonly IQuestionRepository _qRepo;
        private readonly IAnswerRepository _aRepo;
        private readonly IMapper _mapper;

        public SubmitExerciseCommandHandler(
            IStudentExerciseRepository seRepo,
            IStudentAnswerRepository saRepo,
            IQuestionRepository qRepo,
            IAnswerRepository aRepo,
            IMapper mapper)
        {
            _seRepo = seRepo;
            _saRepo = saRepo;
            _qRepo = qRepo;
            _aRepo = aRepo;
            _mapper = mapper;
        }

        public async Task<SubmitExerciseResult> Handle(SubmitExerciseCommand request, CancellationToken ct)
        {
            try
            {
                var se = await _seRepo.GetByIdAsync(request.StudentExerciseId);
                if (se == null)
                    return new SubmitExerciseResult { Success = false, Message = "Student exercise not found." };

                if (se.IsCompleted)
                    return new SubmitExerciseResult { Success = false, Message = "Exercise already submitted." };

                var questions = await _qRepo.GetQuestionsByExerciseIdAsync(se.ExerciseId);
                if (!questions.Any())
                    return new SubmitExerciseResult { Success = false, Message = "No questions in exercise." };

                var studentAnswers = new List<StudentAnswer>();
                decimal totalScore = 0;

                foreach (var dto in request.Answers)
                {
                    var question = questions.FirstOrDefault(q => q.Id == dto.QuestionId);
                    if (question == null) continue;

                    var isCorrect = false;
                    if (dto.AnswerId.HasValue)
                    {
                        var correctAnswers = await _aRepo.GetAnswersByQuestionIdAsync(dto.QuestionId);
                        isCorrect = correctAnswers.Any(a => a.Id == dto.AnswerId.Value && a.IsCorrect);
                    }

                    var sa = new StudentAnswer
                    {
                        Id = Guid.NewGuid(),
                        StudentExerciseId = se.Id,
                        QuestionId = dto.QuestionId,
                        AnswerId = dto.AnswerId ?? Guid.Empty,
                        AnswerText = dto.AnswerText,
                        IsCorrect = isCorrect,
                        SubmittedAt = DateTime.UtcNow
                    };
                    sa.RaiseSubmittedEvent();
                    studentAnswers.Add(sa);

                    if (isCorrect) totalScore += question.Score;
                }

                se.EndTime = DateTime.UtcNow;
                se.Score = totalScore;
                se.IsCompleted = true;
                se.RaiseSubmittedEvent();

                await _saRepo.AddRangeAsync(studentAnswers);
                await _seRepo.UpdateAsync(se);

                var resultDto = _mapper.Map<List<StudentAnswerDto>>(studentAnswers);

                return new SubmitExerciseResult
                {
                    Success = true,
                    Message = "Exercise submitted successfully.",
                    StudentExerciseId = se.Id,
                    Score = totalScore,
                    Answers = resultDto
                };
            }
            catch (Exception ex)
            {
                return new SubmitExerciseResult
                {
                    Success = false,
                    Message = $"Error submitting exercise: {ex.Message}"
                };
            }
        }
    }
}