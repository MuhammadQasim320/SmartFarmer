namespace SmartFarmer.Domain.Model
{
    public class MobileActionType
    {
        public int MobileActionTypeId { get; set; }
        public string Type { get; set; }

        //FK
        public ICollection<AlarmAction> AlarmActions { get; set; }
    }
}
