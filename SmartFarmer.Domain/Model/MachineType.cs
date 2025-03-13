namespace SmartFarmer.Domain.Model
{
    public class MachineType
    {
        public Guid MachineTypeId { get; set; }
        public string Name { get; set; }
        public bool NeedsTraining { get; set; }
        public DateTime CreatedDate { get; set; }

        //FK
        public int UnitsTypeId { get; set; }
        public UnitsType UnitsType { get; set; }
        public Guid? RiskAssessmentId { get; set; }
        public Guid? TrainingId { get; set; }
        public Training Training { get; set; }
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public RiskAssessment RiskAssessment { get; set; }
        public ICollection<CheckList> CheckLists { get; set; }
        public ICollection<Machine> Machines { get; set; }

    }
}
