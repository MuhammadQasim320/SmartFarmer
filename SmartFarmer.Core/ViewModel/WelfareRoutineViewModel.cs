using System.ComponentModel.DataAnnotations;

namespace SmartFarmer.Core.ViewModel
{
    public class WelfareRoutineResponseViewModel : WelfareRoutineRequestViewModel
    {
        public Guid WelfareRoutineId { get; set; }
        public string GroupName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }

    public class WelfareRoutineRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public int Minutes { get; set; }
      
        public Guid? UserGroupId { get; set; }
    }

    public class SearchWelfareRoutineRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
    }

    public class WelfareRoutineCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<WelfareRoutineResponseViewModel> List { get; set; }
    }
}
