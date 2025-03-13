namespace SmartFarmer.Core.ViewModel
{
    public class TrainingFileViewModel
    {
        public string FileName { get; set; }
        public string FileUniqueName { get; set; }
        public string FileUrl { get; set; }
        public Guid TrainingId { get; set; }
        public Guid TrainingFileId { get; set; }
    }
}
