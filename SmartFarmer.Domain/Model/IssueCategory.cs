namespace SmartFarmer.Domain.Model
{
    public class IssueCategory
    {
        public Guid IssueCategoryId { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }

        //FK
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Issue> Issues { get; set; }

    }
}
