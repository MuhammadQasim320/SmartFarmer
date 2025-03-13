namespace SmartFarmer.Domain.Model
{
    public class AlarmAction
    {
        public Guid AlarmActionId { get; set; }
        public string MobileNumber { get; set; }
        public bool SMS { get; set; }
        public bool MakeSound { get; set; }
        public string SmsNumber { get; set; }
        public DateTime CreatedAt {  get; set; } 
        public DateTime UpdatedAt {  get; set; } 

        //Fk
        public MobileActionType MobileActionTypes { get; set; }
        public int MobileActionTypeId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string CreatedBy { get; set; }
    }
}