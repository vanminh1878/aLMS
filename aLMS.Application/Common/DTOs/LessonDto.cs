using System;

namespace aLMS.Application.Common.Dtos
{
    public class LessonDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public Guid TopicId { get; set; }
    }

    public class CreateLessonDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public Guid TopicId { get; set; }
    }

    public class UpdateLessonDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public Guid TopicId { get; set; }
    }
}