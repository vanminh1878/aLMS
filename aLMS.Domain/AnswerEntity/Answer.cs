using aLMS.Domain.Common;
using aLMS.Domain.QuestionEntity;
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
    }
}
