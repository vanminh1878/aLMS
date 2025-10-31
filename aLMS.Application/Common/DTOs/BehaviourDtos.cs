// aLMS.Application.Common.Dtos/BehaviourDtos.cs
using System;

namespace aLMS.Application.Common.Dtos
{
    public class BehaviourDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Video { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public int Order { get; set; }
        public DateTime Date { get; set; }
    }

    public class CreateBehaviourDto
    {
        public Guid StudentId { get; set; }
        public string Video { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public int Order { get; set; }
        public DateTime Date { get; set; }
    }

    public class UpdateBehaviourDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string Video { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public int Order { get; set; }
        public DateTime Date { get; set; }
    }
}