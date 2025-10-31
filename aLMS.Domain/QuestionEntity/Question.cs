using aLMS.Domain.AnswerEntity;
using aLMS.Domain.Common;
using aLMS.Domain.ExerciseEntity;
using aLMS.Domain.StudentAnswerEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.QuestionEntity
{
    public class Question : Entity
    {
        public Guid ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        public string QuestionContent { get; set; }
        public string QuestionImage { get; set; }
        public string QuestionType { get; set; }
        public int OrderNumber { get; set; }
        public decimal Score { get; set; }
        public string Explanation { get; set; }

        public ICollection<Answer> Answers { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; }
        public void RaiseQuestionCreatedEvent()
        {
            AddDomainEvent(new QuestionCreatedEvent(Id, QuestionContent, ExerciseId));
        }

        public void RaiseQuestionUpdatedEvent()
        {
            AddDomainEvent(new QuestionUpdatedEvent(Id, QuestionContent, ExerciseId));
        }

        public void RaiseQuestionDeletedEvent()
        {
            AddDomainEvent(new QuestionDeletedEvent(Id));
        }
    }
}
