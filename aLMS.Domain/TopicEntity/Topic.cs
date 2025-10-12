using aLMS.Domain.Common;
using aLMS.Domain.LessonEntity;
using aLMS.Domain.SubjectEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.TopicEntity
{
    public class Topic : Entity
    {
        public string Title { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
