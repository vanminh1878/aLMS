namespace aLMS.Application.Common.Dtos
{
    public class FinalTermRecordDto
    {
        public Guid Id { get; set; }
        public Guid StudentProfileId { get; set; }
        public string? StudentName { get; set; }     
        public Guid SubjectId { get; set; }           
        public string? SubjectName { get; set; }
        public Guid? ClassId { get; set; }
        public string? ClassName { get; set; }

        public decimal? FinalScore { get; set; }
        public string? FinalEvaluation { get; set; }
        public string? Comment { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateFinalTermRecordDto
    {
        public Guid StudentProfileId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid SubjectId { get; set; }
        public decimal? FinalScore { get; set; }
        public string? FinalEvaluation { get; set; }
        public string? Comment { get; set; }
    }

    public class UpdateFinalTermRecordDto
    {
        public Guid Id { get; set; }
        public decimal? FinalScore { get; set; }
        public string? FinalEvaluation { get; set; }
        public string? Comment { get; set; }
    }
}