using System.ComponentModel.DataAnnotations;

namespace SmartFarmer.Core.ViewModel
{
    public class RiskAssessmentViewModel : RiskAssessmentRequestViewModel
    {
        public Guid RiskAssessmentId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
    public class RiskAssessmentRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public int? Validity { get; set; }

    }
    public class RiskAssessmentSearchRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }

    }
    public class RiskAssessmentSearchResponseViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<RiskAssessmentWithFileURLsResponseViewModel> List { get; set; }
    }

    public class RiskAssessmentFileViewModel
    {
        public string FileName { get; set; }
        public string FileUniqueName { get; set; }
        public string FileUrl { get; set; }
        public Guid RiskAssessmentId { get; set; }
        public Guid RiskAssessmentFileId { get; set; }
    }
    
    public class RiskAssessmentDetailViewModel
    {
        public RiskAssessmentViewModel Detail { get; set; }
        public List<RiskAssessmentFileViewModel> Files { get; set; }
    }
    public class RiskAssessmentResponseViewModel : RiskAssessmentViewModel
    {
        public List<RiskAssessmentFileViewModel> RiskAssessmentFiles { get; set; }
    }
    public class RiskAssessmentWithFileURLsResponseViewModel : RiskAssessmentViewModel
    {
        public List<string> Files { get; set; }
    }

    public class RiskAssessmentNameListViewModel
    {
        public List<RiskAssessmentNameViewModel> List { get; set; }
    }
    public class RiskAssessmentNameViewModel
    {
        public Guid RiskAssessmentId { get; set; }
        public string Name { get; set; }
        public int? Validity { get; set; }
    }

    public class RiskAssessmentLogViewModel : RiskAssessmentLogRequestViewModel
    {
        public Guid RiskAssessmentLogId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string InitialRiskName { get; set; }
        public string AdjustedRiskName { get; set; }
        public string RiskAssessmentName { get; set; }
        public DateTime? ExpiresAt { get; set; }
        //public string ActionName { get; set; }
        public bool Expired { get; set; }
        public int? Validity { get; set; }
        public int Open { get; set; }
        public int Complete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

    }

    public class RiskAssessmentLogRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public Guid? RiskAssessmentId { get; set; }
        //public DateTime? CompletedDate { get; set; }
        public bool Expires { get; set; }
        public int? InitialRiskId { get; set; }
        public int? AdjustedRiskId { get; set; }
        public bool Archived { get; set; }
        //public Guid? ActionId { get; set; }
    }
    public class RiskAssessmentLogSearchRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
        public bool? Archived { get; set; }
        public bool? Expires { get; set; }
        public Guid? RiskAssessmentId { get; set; }
    }
    public class RiskAssessmentLogSearchResponseViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<RiskAssessmentLogViewModel> List { get; set; }
    }


    public class RiskAssessmentLogNameViewModel
    {
        public Guid RiskAssessmentLogId { get; set; }
        public string Name { get; set; }
    }

    public class RiskAssessmentLogNameListViewModel
    {
        public List<RiskAssessmentLogNameViewModel> List { get; set; }
    }


    public class RiskAssessmentWithLogDetailViewModel
    {
        public RiskAssessmentViewModel Detail { get; set; }
        public List<RiskAssessmentFileViewModel> Files { get; set; }
        public RiskAssessmentLogViewModel Log { get; set; }   
    }
}
