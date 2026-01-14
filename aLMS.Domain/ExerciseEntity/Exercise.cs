using aLMS.Domain.Common;
using aLMS.Domain.QuestionEntity;
using aLMS.Domain.StudentExerciseEntity;
using aLMS.Domain.TopicEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.ExerciseEntity
{
    public class Exercise : Entity
    {
        public string Title { get; set; }
        public string? ExerciseFile { get; set; }
        public bool HasTimeLimit { get; set; } = false;
        public int? TimeLimit { get; set; }
        public string Type { get; set; }
        public string QuestionLayout { get; set; }
        public int OrderNumber { get; set; }
        public decimal TotalScore { get; set; }

        public Guid TopicId { get; set; }
        public Topic Topic { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<StudentExercise> StudentExercises { get; set; }
        public void RaiseExerciseCreatedEvent()
        {
            AddDomainEvent(new ExerciseCreatedEvent(Id, Title, TopicId));
        }

        public void RaiseExerciseUpdatedEvent()
        {
            AddDomainEvent(new ExerciseUpdatedEvent(Id, Title, TopicId));
        }

        public void RaiseExerciseDeletedEvent()
        {
            AddDomainEvent(new ExerciseDeletedEvent(Id));
        }
    }
}
