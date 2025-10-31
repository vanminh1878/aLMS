// aLMS.Application.Common.Dtos/QuestionDtos.cs
using System;
using System.Collections.Generic;

namespace aLMS.Application.Common.Dtos
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
        public string? QuestionImage { get; set; }
        public string QuestionType { get; set; } = string.Empty;
        public int OrderNumber { get; set; }
        public decimal Score { get; set; }
        public string? Explanation { get; set; }
        public Guid ExerciseId { get; set; }
        public List<AnswerDto> Answers { get; set; } = new();
    }

    public class CreateQuestionDto
    {
        public string QuestionContent { get; set; } = string.Empty;
        public string? QuestionImage { get; set; }
        public string QuestionType { get; set; } = string.Empty;
        public int OrderNumber { get; set; }
        public decimal Score { get; set; }
        public string? Explanation { get; set; }
        public Guid ExerciseId { get; set; }
        public List<CreateAnswerDto> Answers { get; set; } = new();
    }

    public class UpdateQuestionDto
    {
        public Guid Id { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
        public string? QuestionImage { get; set; }
        public string QuestionType { get; set; } = string.Empty;
        public int OrderNumber { get; set; }
        public decimal Score { get; set; }
        public string? Explanation { get; set; }
        public Guid ExerciseId { get; set; }
        public List<UpdateAnswerDto> Answers { get; set; } = new();
    }
}