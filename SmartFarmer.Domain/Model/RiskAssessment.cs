namespace SmartFarmer.Domain.Model
{
    public class RiskAssessment
    {
        public Guid RiskAssessmentId { get; set; }
        public string Name { get; set; }
        public int? Validity { get; set; }
        public DateTime CreatedDate { get; set; }

        //FK
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //public ICollection<Action> Actions { get; set; }
        public ICollection<RiskAssessmentFile> RiskAssessmentFiles { get; set; }
        public ICollection<MachineType> MachineTypes { get; set; }
        public ICollection<RiskAssessmentLog> RiskAssessmentLogs { get; set; }
    }
}
