using System.ComponentModel.DataAnnotations;

namespace SmartFarmer.Core.ViewModel
{
    public class UserGroupResponseViewModel : UserGroupRequestViewModel
    {
        public Guid UserGroupId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }
    public class UserGroupRequestViewModel
    {
        public string GroupName { get; set; }
        public string Description { get; set; }
    }

    public class SearchUserGroupRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
    }

    public class UserGroupCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<UserGroupResponseViewModel> List { get; set; }
    }

    public class UserGroupNameListViewModel
    {
        public Guid UserGroupId { get; set; }
        public string UserGroupName { get; set; }

    }
    public class UserGroupListViewModel
    {
        public IEnumerable<UserGroupNameListViewModel> UserGroupList { get; set; }
    }
}
