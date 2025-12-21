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
        //public Guid QuestionId { get; set; }
        //public Guid? AnswerId { get; set; }
        //public string? AnswerText { get; set; }
        //public bool IsCorrect { get; set; }
        //public DateTime SubmittedAt { get; set; }
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
        public string? QuestionImage { get; set; }
        public string QuestionType { get; set; } = string.Empty;
        public decimal Score { get; set; } // Điểm câu hỏi

        public Guid? AnswerId { get; set; }
        public string? SelectedAnswerContent { get; set; }
        public string? AnswerText { get; set; } // Tự luận

        public List<string> CorrectAnswerContents { get; set; } = new();
        public string? Explanation { get; set; }

        public bool IsCorrect { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}