using System;

namespace aLMS.Application.Common.Dtos
{
    public class AnswerDto
    {
        public Guid Id { get; set; }
        public string AnswerContent { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderNumber { get; set; }
        public Guid QuestionId { get; set; }
    }

    public class CreateAnswerDto
    {
        public string AnswerContent { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderNumber { get; set; }
    }

    public class UpdateAnswerDto
    {
        public Guid Id { get; set; }
        public string AnswerContent { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderNumber { get; set; }
    }
}