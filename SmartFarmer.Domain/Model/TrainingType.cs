namespace SmartFarmer.Domain.Model
{
    public class TrainingType
    {
        public int TrainingTypeId { get; set; }
        public string Type { get; set; }

        //FK
        public ICollection<Training> Trainings { get; set; }
        public ICollection<TrainingRecord> TrainingRecords { get; set; }

    }
}
