using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.StudentQualityEvaluationEntity
{
    public class StudentQualityEvaluationCreatedEvent : IDomainEvent
    {
        public Guid Id { get; }
        public Guid StudentEvaluationId { get; }
        public Guid QualityId { get; }

        public StudentQualityEvaluationCreatedEvent(Guid id, Guid studentEvaluationId, Guid qualityId)
        {
            Id = id;
            StudentEvaluationId = studentEvaluationId;
            QualityId = qualityId;
        }
    }
}