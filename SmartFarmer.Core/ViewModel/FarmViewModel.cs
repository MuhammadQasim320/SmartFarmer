using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SmartFarmer.Core.ViewModel
{
    public class FarmViewModel
    {
        public Guid FarmId { get; set; }
        public string FarmName { get; set; }
        public string CreatedBy { get; set; }
        public string MasterAdminId { get; set; }
    }
    public class FarmResponseViewModel
    {
        public Guid FarmId { get; set; }
        public string FarmName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string MasterAdminId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsWorking { get; set; }
    }

    public class FarmRequestViewModel
    {
        [MaxLength(1000)]
        public string FarmName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
    }

    public class SearchFarmRequestViewModel
    {
        public string SearchKey { get; set; }
    } 
    
    public class FarmOperatorMappingViewModel
    {
        [Key]
        public Guid FarmOperatorMappingId { get; set; }
        //FK
        public Guid FarmId { get; set; }
        public string OperatorId { get; set; }
    }

    public class UpdateFarmOperatorMappingViewModel
    {
        [Key]
        public Guid FarmOperatorMappingId { get; set; }
        //FK
        public Guid FarmId { get; set; }
        public string OperatorId { get; set; }
        public Guid? UserGroupId { get; set; }
        public int ApplicationUserTypeId { get; set; }
    }
    public class CreateFarmOperatorMappingViewModel
    {
        [Key]
        public Guid FarmOperatorMappingId { get; set; }
        //FK
        public Guid FarmId { get; set; }
        public string OperatorId { get; set; }
        public Guid? UserGroupId { get; set; }
        public int ApplicationUserTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class FarmCountRequestViewModel
    {
        public IEnumerable<FarmResponseViewModel> List { get; set; }
    } 
    public class FarmUserRequestViewModel
    {
        public Guid FarmId { get; set; }
        public string Email { get; set; }
    }

    public class AddAlarmActionRequestViewModel
    {
        public Guid? AlarmActionId { get; set; }
        [MaxLength(100)]
        public string MobileNumber { get; set; }
        public bool SMS { get; set; }
        public bool MakeSound { get; set; }
        public int MobileActionTypeId { get; set; }
        public List<SmsNumberViewModel> SmsNumbers { get; set; }
     }

    public class SmsNumberViewModel
    {
        public string SmsNumber { get; set; }
    }

public class AddAlarmActionResponseViewModel: AddAlarmActionRequestViewModel
    {
        //public Guid AlarmActionId { get; set; }
        //public string MobileNumber { get; set; }
        //public bool SMS { get; set; }
        //public bool MakeSound { get; set; }
        //public int MobileActionTypeId { get; set; }
        //public string SmsNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string MobileActionTypeName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }



    public class FarmSearchRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
    }

    public class CountFarmResponseViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<FarmDetailViewModel> List { get; set; }
    }

    public class UpdateFarmRequestViewModel
    {
        [MaxLength(1000)]
        public string FarmName { get; set; }
        public string MasterAdminFirstName { get; set; }
        public string MasterAdminLastName { get; set; }
    }


    public class FarmDetailViewModel
    {
        public Guid FarmId { get; set; }
        public string FarmName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string MasterAdminId { get; set; }
        public string MasterAdminFirstName { get; set; }
        public string MasterAdminLastName { get; set; }
        public string MasterAdminEmail { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class FarmNameListViewModel
    {
        public List<FarmNameViewModel> List { get; set; }
    }
    public class FarmNameViewModel
    {
        public Guid FarmId { get; set; }
        public string FarmName { get; set; }
    }

    public class AccessFarmUserRequestViewModel
    {
        public Guid FarmId { get; set; }
        public string Email { get; set; }
    }
}
