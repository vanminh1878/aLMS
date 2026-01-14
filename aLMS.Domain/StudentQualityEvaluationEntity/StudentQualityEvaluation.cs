using aLMS.Domain.Common;
using aLMS.Domain.QualityEntity;
using aLMS.Domain.StudentEvaluationEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.StudentQualityEvaluationEntity
{
    public class StudentQualityEvaluation : Entity
    {
        public Guid StudentEvaluationId { get; set; }
        public StudentEvaluation StudentEvaluation { get; set; }

        public Guid QualityId { get; set; }
        public Quality Quality { get; set; }

        public string QualityLevel { get; set; } // Tốt, Khá,...
    }
}
