using aLMS.Domain.Common;
using aLMS.Domain.ExerciseEntity;
using aLMS.Domain.StudentAnswerEntity;
using aLMS.Domain.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.StudentExerciseEntity
{
    public class StudentExercise : Entity
    {
        public Guid StudentId { get; set; }
        public User Student { get; set; }

        public Guid ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Score { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int AttemptNumber { get; set; }

        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
