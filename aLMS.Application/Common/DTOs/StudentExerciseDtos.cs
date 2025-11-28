using System;
using System.Collections.Generic;

namespace aLMS.Application.Common.Dtos
{
    public class StartExerciseDto
    {
        public Guid ExerciseId { get; set; }
    }

    public class SubmitAnswerDto
    {
        public Guid QuestionId { get; set; }
        public Guid? AnswerId { get; set; } 
        public string? AnswerText { get; set; } 
    }

    public class SubmitExerciseDto
    {
        public List<SubmitAnswerDto> Answers { get; set; } = new();
    }

    public class StudentExerciseDto
    {
        public Guid Id { get; set; }
        public Guid ExerciseId { get; set; }
        public string ExerciseTitle { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal Score { get; set; }
        public bool IsCompleted { get; set; }
        public int AttemptNumber { get; set; }
        public List<StudentAnswerDto> Answers { get; set; } = new();
    }

    public class StudentAnswerDto
    {
        public Guid QuestionId { get; set; }
        public Guid? AnswerId { get; set; }
        public string? AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}