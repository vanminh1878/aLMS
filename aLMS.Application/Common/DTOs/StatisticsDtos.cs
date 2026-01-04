using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.Common.DTOs
{
    public class TeacherCountByDepartmentDto
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int TeacherCount { get; set; }
    }

    public class ExerciseCompletionRateDto
    {
        public string ClassName { get; set; } = string.Empty;
        public int TotalExercises { get; set; }
        public int AvgCompletedExercises { get; set; }
        public double CompletionRate { get; set; } 
    }

    public class StudentCountByGradeDto
    {
        public string Grade { get; set; } = string.Empty; 
        public int StudentCount { get; set; }
    }
}
