namespace SmartFarmer.Domain.Model
{
    public class ApplicationUserStatus
    {
        public int ApplicationUserStatusId { get; set; }
        public string Status { get; set; }

        //FK
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
