using System.ComponentModel.DataAnnotations;

namespace SmartFarmer.Core.ViewModel
{
    public class MachineTypeViewModel : MachineTypeRequestViewModel
    {
        public Guid MachineTypeId { get; set; }
        public string TrainingName { get; set; }
        public string Units { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string RiskAssessmentName { get; set; }
    }
    public class MachineTypeRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public bool NeedsTraining { get; set; }
        public Guid? TrainingId { get; set; }
      
        public Guid? RiskAssessmentId { get; set; }
       
        public int UnitsTypeId { get; set; }

    }
    public class MachineTypeSearchRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
        public Guid? RiskAssessmentId { get; set; }
        public bool? NeedsTraining { get; set; }

    }
    public class MachineTypeSearchResponseViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<MachineTypeViewModel> List { get; set; }

    }
    
    public class MachineTypeNameListViewModel
    {
        public List<MachineTypeNameViewModel> List { get; set; }
    }
    
    public class MachineTypeNameViewModel
    {
        public Guid MachineTypeId { get; set; }
        public string MachineTypeName { get; set; }
        public int UnitTypeId { get; set; }
        public string UnitName { get; set; }
    }
}
