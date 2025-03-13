using System.ComponentModel.DataAnnotations;
using System.Runtime;

namespace SmartFarmer.Core.ViewModel
{
    public class CheckListViewModel : CheckListRequestViewModel
    {
        public Guid CheckListId { get; set; }
        public string FrequencyType { get; set; }
        public string CheckType { get; set; }
        public string MachineTypeName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string CheckListStatus { get; set; }
        public DateTime? LastCheckDate { get; set; }
        public int? ResultId { get; set; }
        public string Result { get; set; }


    }
    public class CheckListRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Frequency { get; set; }
        [Required]
        public Guid MachineTypeId { get; set; }
        [Required]
        public int FrequencyTypeId { get; set; }
        [Required]
        public int CheckTypeId { get; set; }

    }
    public class OperatorCheckListResponseViewModel
    {
        public Guid CheckListId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public DateTime? LastCheckDate { get; set; }
        public DateTime? NextDueDate { get; set; }

    }
    public class OperatorCheckListRequestViewModel
    {
        public DateTime? LastCheckDate { get; set; }
        public DateTime? NextDueDate { get; set; }

    }
    public class CheckListSearchRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
        public int? FrequencyTypeId { get; set; }
        public int? CheckTypeId { get; set; }
        public Guid? MachineTypeId { get; set; }

    }
    public class CheckListSearchResponseViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<CheckListViewModel> List { get; set; }

    }
    public class CheckListNameListViewModel
    {
        public Guid CheckListId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

    }
    public class CheckListListViewModel
    {
        public IEnumerable<CheckListNameListViewModel> CheckListList { get; set; }
    }
    public class CheckListItemViewModel : CheckListItemRequestViewModel
    {
        public Guid CheckListItemId { get; set; }
    }
    public class CheckListItemRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public string Instruction { get; set; }
        public string Order { get; set; }
        public int Priority { get; set; }
        [Required]
        public Guid CheckListId { get; set; }

    }
    public class GetCheckListItemsViewModel
    {
        public List<CheckListItemViewModel> List { get; set; }

        public GetCheckListItemsViewModel()
        {
            List = new List<CheckListItemViewModel>();
        }
    }
    public class CheckListItemsListViewModel
    {
        public Guid? CheckListItemId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Instruction { get; set; }
        public string Order { get; set; }
        public int Priority { get; set; }
    }
    public class CheckListItemListViewModel
    {
        public string Name { get; set; }
        public Guid CheckListId { get; set; }
        public IEnumerable<CheckListItemViewModel> CheckListItemList { get; set; }
    }
    public class StartCheckListViewModel
    {
        public Guid MachineId { get; set; }
        public string Location { get; set; }
        public Guid CheckListId { get; set; }
        public List<CheckListItemAnswerViewModel> Items { get; set; }
    }
    public class CheckListItemAnswerViewModel
    {
        public Guid CheckListItemId { get; set; }
        public string Answer { get; set; }
    }
    public class StartCheckListResponseViewModel
    {
        public Guid CheckListMachineMappingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public Guid CheckListId { get; set; }
        public string CheckListName { get; set; }
        public Guid MachineId { get; set; }
        public string MachineName { get; set; }
        public int ResultId { get; set; }
        public string Result { get; set; }
        public List<CheckListItemAnswerResponseViewModel> ChecListItemAnswerMappings { get; set; }
    }
    public class CheckListItemAnswerResponseViewModel
    {
        public Guid ChecListItemAnswerMappingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Answer { get; set; }
        public Guid CheckListItemId { get; set; }
    }

    public class PreCheckLogsRequestViewModel
    {
        public string SearchKey { get; set; }
        public int? FrequencyTypeId { get; set; }
        public int? CheckListStatusId { get; set; }
        public Guid? MachineId { get; set; }
        public bool? ThisWeek { get; set; }

    }

    public class PreCheckLogsResponseViewModel
    {
        public List<GetMachineCheckWithMachineViewModel> List { get; set; } =  new List<GetMachineCheckWithMachineViewModel>(); 
        //public int TotalCount { get; set; }
    }
    public class GetMachineCheckWithMachineViewModel
    {
        public CheckListViewModel CheckListDetails { get; set; } = new CheckListViewModel();
        public MachineDetailViewModel MachineDetails { get; set; } = new MachineDetailViewModel();
    }
    public class MachineCheckListViewModel
    {
        public Guid CheckListId { get; set; }
        public string Name { get; set; }
        public DateTime? LastCheckDate { get; set; }
        public int CheckTypeId { get; set; }
        public string CheckType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }
    public class MachineCheckListListViewModel
    {
        public IEnumerable<MachineCheckListViewModel> List { get; set; }
    }
}
