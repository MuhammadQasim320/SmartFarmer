namespace SmartFarmer.Domain.Model
{
    public class UserGroup
    {
        public Guid UserGroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        //FK
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<WelfareRoutine> WelfareRoutines { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }
}
