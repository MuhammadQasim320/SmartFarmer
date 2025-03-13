namespace SmartFarmer.Domain.Model
{
    public class CheckType
    {
        public int CheckTypeId { get; set; }
        public string Type { get; set; }

        //FK
        public ICollection<CheckList> CheckLists { get; set; }
    }
}
