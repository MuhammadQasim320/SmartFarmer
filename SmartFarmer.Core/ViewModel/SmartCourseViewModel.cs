//using System.ComponentModel.DataAnnotations;
//using System.Runtime;

//namespace SmartFarmer.Core.ViewModel
//{
//    public class SmartCourseViewModel : SmartCourseRequestViewModel
//    {
//        public Guid SmartCourseId { get; set; }
//        public string CreatedBy { get; set; }
//        public string CreatedByName { get; set; }
//    }
//    public class SmartCourseRequestViewModel
//    {
//        [MaxLength(100)]
//        public string Name { get; set; }

//    }
//    public class SmartCourseSearchRequestViewModel
//    {
//        public int PageNumber { get; set; }
//        public int PageSize { get; set; }
//        public string searchKey { get; set; }
//    }
//    public class SmartCourseSearchResponseViewModel
//    {
//        public int TotalCount { get; set; }
//        public IEnumerable<SmartCourseViewModel> List { get; set; }
//    }
    
//    public class SmartCourseNameListViewModel
//    {
//        public List<SmartCourseNameViewModel> List { get; set; }
//    }
//    public class SmartCourseNameViewModel
//    {
//        public Guid SmartCourseId { get; set; }
//        public string SmartCourseName { get; set; }
//    }
//}
