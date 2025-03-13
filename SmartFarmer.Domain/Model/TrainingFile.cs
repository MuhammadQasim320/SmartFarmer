namespace SmartFarmer.Domain.Model
{
    public class TrainingFile
    {
        public Guid TrainingFileId { get; set; }
        public string FileUrl { get; set; }
        public string FileUniqueName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FileName { get; set; }

        //FK
        public Guid TrainingId { get; set; }
        public Training training { get; set; }
    }
}
