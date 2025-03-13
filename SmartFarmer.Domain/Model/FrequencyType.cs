namespace SmartFarmer.Domain.Model
{
    public class FrequencyType
    {
        public int FrequencyTypeId { get; set; }
        public string Type { get; set; }

        //FK
        public ICollection<CheckList> CheckLists { get; set; }
    }
}
