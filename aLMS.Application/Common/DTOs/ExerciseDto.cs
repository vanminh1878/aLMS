using System;

namespace aLMS.Application.Common.Dtos
{
    public class ExerciseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ExerciseFile { get; set; } = string.Empty;
        public bool HasTimeLimit { get; set; }
        public int? TimeLimit { get; set; }
        public string QuestionLayout { get; set; } = string.Empty;
        public int OrderNumber { get; set; }
        public decimal TotalScore { get; set; }
        public Guid TopicId { get; set; }
        public Guid ClassId { get; set; }
    }

    public class CreateExerciseDto
    {
        public string Title { get; set; } = string.Empty;
        public string ExerciseFile { get; set; } = string.Empty;
        public bool HasTimeLimit { get; set; }
        public int? TimeLimit { get; set; }
        public string QuestionLayout { get; set; } = string.Empty;
        public int OrderNumber { get; set; }
        public decimal TotalScore { get; set; }
        public Guid TopicId { get; set; }
    }

    public class UpdateExerciseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ExerciseFile { get; set; } = string.Empty;
        public bool HasTimeLimit { get; set; }
        public int? TimeLimit { get; set; }
        public string QuestionLayout { get; set; } = string.Empty;
        public int OrderNumber { get; set; }
        public decimal TotalScore { get; set; }
        public Guid TopicId { get; set; }
    }
}