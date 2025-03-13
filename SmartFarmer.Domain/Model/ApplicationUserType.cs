namespace SmartFarmer.Domain.Model
{
    public class ApplicationUserType
    {
        public int ApplicationUserTypeId { get; set; }
        public string Type { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
