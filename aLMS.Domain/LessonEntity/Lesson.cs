using aLMS.Domain.Common;
using aLMS.Domain.ExerciseEntity;
using aLMS.Domain.TopicEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.LessonEntity
{
    public class Lesson : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ResourceType { get; set; }
        public string Content { get; set; }
        public bool IsRequired { get; set; } = false;

        public Guid TopicId { get; set; }
        public Topic Topic { get; set; }

        public ICollection<Exercise> Exercises { get; set; }
    }
}
