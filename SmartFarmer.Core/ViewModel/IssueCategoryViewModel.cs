using System.ComponentModel.DataAnnotations;

namespace SmartFarmer.Core.ViewModel
{
    public class IssueCategoryResponseViewModel : IssueCategoryRequestViewModel
    {
        public Guid IssueCategoryId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }

    public class IssueCategoryRequestViewModel
    {
        [MaxLength(100)]
        public string Category { get; set; }
    }

    public class SearchIssueCategoryRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
    }

    public class IssueCategoryCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<IssueCategoryResponseViewModel> List { get; set; }
    }

    public class IssueCategoryNameListViewModel
    {
        public List<IssueCategoryNameViewModel> List { get; set; }
    }
    public class IssueCategoryNameViewModel
    {
        public Guid IssueCategoryId { get; set; }
        public string IssueCategoryName { get; set; }
    }
}
