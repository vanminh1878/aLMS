using aLMS.Domain.Common;
using aLMS.Domain.StudentEvaluationEntity;
using aLMS.Domain.SubjectEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.StudentSubjectCommentEntity
{
    public class StudentSubjectComment : Entity
    {
        public Guid StudentEvaluationId { get; set; }
        public StudentEvaluation StudentEvaluation { get; set; }

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string Comment { get; set; }
    }
}
