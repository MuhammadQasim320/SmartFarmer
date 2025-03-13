namespace SmartFarmer.Domain.Model
{
    public class WelfareRoutine
    {
        public Guid WelfareRoutineId { get; set; }
        public string Name { get; set; }
        public int Minutes { get; set; }
        public DateTime CreatedDate { get; set; }

        //FK
        public Guid? UserGroupId { get; set; }
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public UserGroup UserGroup { get; set; }
    }
}
