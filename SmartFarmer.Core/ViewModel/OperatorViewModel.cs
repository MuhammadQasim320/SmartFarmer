namespace SmartFarmer.Core.ViewModel
{
    public class OperatorViewModel
    {
        public class SearchOperatorRequestViewModel
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public string SearchKey { get; set; }
            public int? OperatorStatusId { get; set; }
            public Guid? UserGroupId { get; set; }
        }

        public class OperatorListViewModel
        {
            public int TotalCount { get; set; }
            public IEnumerable<ApplicationUserDetailsViewModel> List { get; set; }
        }



         public class NotificationResponseViewModel 
        {
            public Guid NotificationId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
            public bool IsRead { get; set; }
            public string ToId { get; set; }
        }


    }
}
