using aLMS.Domain.Common;
using aLMS.Domain.QuestionEntity;
using aLMS.Domain.StudentAnswerEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.AnswerEntity
{
    public class Answer : Entity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public string AnswerContent { get; set; }
        public bool IsCorrect { get; set; } = false;
        public int OrderNumber { get; set; }

        public ICollection<StudentAnswer> StudentAnswers { get; set; }
        public void RaiseAnswerCreatedEvent()
        {
            AddDomainEvent(new AnswerCreatedEvent(Id, AnswerContent, IsCorrect, QuestionId));
        }

        public void RaiseAnswerUpdatedEvent()
        {
            AddDomainEvent(new AnswerUpdatedEvent(Id, AnswerContent, IsCorrect, QuestionId));
        }

        public void RaiseAnswerDeletedEvent()
        {
            AddDomainEvent(new AnswerDeletedEvent(Id));
        }
    }
}
