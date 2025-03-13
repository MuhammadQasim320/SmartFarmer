namespace SmartFarmer.Domain.Model
{
    public class Training
    {
        public Guid TrainingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Expires { get; set; }
        public int? Validity { get; set; }
        public bool IsArchived { get; set; }
        public string Certification { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Link { get; set; }
        //FK
        //public Guid? SmartCourseId { get; set; }
        public int TrainingTypeId { get; set; }
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //public SmartCourse SmartCourse { get; set; }
        public TrainingType TrainingType { get; set; }
        public ICollection<MachineType> MachineTypes { get; set; }
        public ICollection<TrainingFile> TrainingFiles { get; set; }
        public ICollection<TrainingOperatorMapping> TrainingOperatorMappings { get; set; }
        public ICollection<SmartQuestion> SmartQuestions { get; set; }
    }
}
