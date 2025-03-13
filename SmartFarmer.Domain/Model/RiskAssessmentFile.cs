namespace SmartFarmer.Domain.Model
{
    public class RiskAssessmentFile
    {
        public Guid RiskAssessmentFileId { get; set; }
        public string FileUrl { get; set; }
        public string FileUniqueName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FileName { get; set; }

        //FK
        public Guid RiskAssessmentId { get; set; }
        public RiskAssessment RiskAssessment { get; set; }
    }
}
