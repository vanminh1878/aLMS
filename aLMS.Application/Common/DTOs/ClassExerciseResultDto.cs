using aLMS.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.Common.DTOs
{
    public class ClassExerciseOverviewDto
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string ExerciseTitle { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int SubmittedCount { get; set; }
        public int NotSubmittedCount { get; set; }
        public decimal AverageScore { get; set; }
        public decimal HighestScore { get; set; }
        public decimal LowestScore { get; set; }

        public List<StudentExerciseSummaryDto> StudentResults { get; set; } = new();
    }

    public class StudentExerciseSummaryDto
    {
        public Guid StudentExerciseId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? Score { get; set; }
        public decimal TotalScore { get; set; }
        public bool IsCompleted { get; set; }
        public int AttemptNumber { get; set; }
    }
    public class StudentExerciseDetailDto : StudentExerciseDto
    {
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public decimal? TotalScore { get; set; }
    }
}
