using System;

namespace aLMS.Application.Common.Dtos
{
    public class TopicDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid SubjectId { get; set; }
    }

    public class CreateTopicDto
    {
        public string Title { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid SubjectId { get; set; }
    }

    public class UpdateTopicDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid SubjectId { get; set; }
    }
}