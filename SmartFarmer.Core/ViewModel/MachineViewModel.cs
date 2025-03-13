using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Runtime;

namespace SmartFarmer.Core.ViewModel
{
    public class MachineResponseViewModel
    {
        public Guid MachineId { get; set; }
        public string MachineImage { get; set; }
        public string MachineImageUniqueName { get; set; }
        public string QRCode { get; set; }
        public string QRUniqueName { get; set; }
        public string NickName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? MOTDate { get; set; }
        public DateTime? LOLERDate { get; set; }
        public int ServiceInterval { get; set; }
        public Guid MachineTypeId { get; set; }
        public string MachineType { get; set; }
        public int MachineStatusId { get; set; }
        public string Status { get; set; }
        public string ApplicationUserId { get; set; }
        public string ApplicationUserName { get; set; }
        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string WorkingIn { get; set; }
        public string MachineCategoryName { get; set; }
        public Guid MachineCategoryId { get; set; }
        public string Location { get; set; }
        public bool InSeason { get; set; }
        public int Warning { get; set; }
        public int Defect { get; set; }
        public int Corrective { get; set; }
        public DateTime? StartOperatingTime { get; set; }
        public int UnitTypeId { get; set; }
        public string Unit { get; set; }
        public long? MachineCode { get; set; }
        public bool NeedsTraining { get; set; }
        public Guid? TrainingId { get; set; }
        public bool HasDoneTraining { get; set; }
        public bool IsDefected { get; set; }
        public bool Archived { get; set; }
        public int ResultId { get; set; }
        public string Result { get; set; }
        public Guid? RiskAssessmentId { get; set; }
        public string RiskAssessmentName { get; set; }
        public int? Validity { get; set; }
        public List<MachineRiskAssessmentFileViewModel> RiskAssessmentFiles { get; set; }

    }
    public class MachineRiskAssessmentFileViewModel
    {
        public Guid? RiskAssessmentFileId { get; set; }
        public string FileUrl { get; set; }
        public string FileUniqueName { get; set; }
        public string FileName { get; set; }
    }
    public class MachineResponseWithDueCheckViewModel 
    {
        public MachineResponseViewModel Detail { get; set; }
        public List<MachineIssueViewModel> IssueDetail { get; set; } = new List<MachineIssueViewModel>();
        public List<GetMachineCheckViewModel> CheckLists { get; set; } = new List<GetMachineCheckViewModel>();
    }
    public class GetMachineCheckViewModel
    {
       
        public CheckListViewModel CheckListDetails { get; set; }
    }
    public class MachineRequestViewModel
    {
        public string NickName { get; set; }
        [Required]
        public string Make { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? MOTDate { get; set; }
        public DateTime? LOLERDate { get; set; }
        [Required]
        public int ServiceInterval { get; set; }
        [Required]
        public Guid MachineTypeId { get; set; }
        public string WorkingIn { get; set; }
        public Guid MachineCategoryId { get; set; }
     
       // public string Location { get; set; }
        [Required]
        public bool InSeason { get; set; }
        public bool Archived { get; set; }
    }

    public class SearchMachineRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
        public int? MachineStatusId { get; set; }
        public Guid? MachineTypeId { get; set; }
        public Guid? MachineCategoryId { get; set; }
        public bool? Archived { get; set; }
        
    }

    public class MachineCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public List<MachineResponseWithDueCheckViewModel> List { get; set; } = new List<MachineResponseWithDueCheckViewModel>();
    }
    
    public class RecentMachineCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public List<MachineResponseWithDueCheckViewModel> List { get; set; } = new List<MachineResponseWithDueCheckViewModel>();
    }

    public class ActiveMachineResponseViewModel
    {
        public List<MachineResponseWithDueCheckViewModel> List { get; set; } = new List<MachineResponseWithDueCheckViewModel>();
    }

    public class MachineNameListViewModel
    {
        public List<MachineNameViewModel> List { get; set; }
    }
    public class MachineNameViewModel
    {
        public Guid MachineId { get; set; }
        public string MachineName { get; set; }
        public string MachineImage { get; set; }
        public string MachineImageUniqueName { get; set; }
    }

    public class MachineNickNameViewModel
    {
        public Guid MachineId { get; set; }
        public string NickName { get; set; }
        public string MachineImage { get; set; }
        public string QRCode { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
    }
    
    public class RecentMachineResponseViewModel
    {
        public Guid MachineId { get; set; }
        public string NickName { get; set; }
        public string MachineImage { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public int MachineStatusId { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public Guid MachineTypeId { get; set; }
        public string MachineTypeName { get; set; }
    }

    public class MachineImageViewModel
    {
        public Guid MachineId { get; set; }
        public IFormFile Image { get; set; }
    }
    
    public class MachineDetailViewModel
    {
        public Guid MachineId { get; set; }
        public string MachineName { get; set; }
        public string NickName { get; set; }
        public Guid MachineTypeId { get; set; }
        public string MachineTypeName { get; set; }
        public int MachineStatusId { get; set; }
        public string MachineStatusName { get; set; }
        public string MachineImage { get; set; }
    }
    public class UpdateMachineStatusRequestlViewModel
    {
        public Guid machineId { get; set; }
        public string operatorId { get; set; }
        public int machineStatusId { get; set; }
        public string ReasonOfServiceRemoval { get; set; }
    }
}
