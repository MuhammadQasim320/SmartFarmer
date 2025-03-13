using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.ViewModel
{
    public class IssueViewModel
    {
        public string IssueTitle { get; set; }
        public string Description { get; set; }
    }
    public class IssueResponseViewModel : IssueRequestViewModel
    {
        public Guid IssueId { get; set; }
        public int IssueNo { get; set; }
        public string CreatedBy { get; set; }
        public string IssueCategoryName { get; set; }
        public string MachineName { get; set; }
        public string MachineNickName { get; set; }
        public string RiskAssessmentLogName { get; set; }
        public string IssueTypeName { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string ResolvedBy { get; set; }
        public string ResolvedByProfile { get; set; }
        public int IssueStatusId { get; set; }
        public string IssueStatusName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public string ResolvedByName { get; set; }
        public Guid? IssueCategoryId { get; set; }
        public int? IssueTypeId { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }

    }
    public class IssueRequestViewModel
    {
        public string IssueTitle { get; set; }
        public string Description { get; set; }
        public Guid? IssueCategoryId { get; set; }
        public int IssueTypeId { get; set; }
        public Guid? MachineId { get; set; }
        public Guid? RiskAssessmentLogId { get; set; }
        public bool IsTargetDateExist { get; set; }
        public DateTime? TargetDate { get; set; }
    }  
    
    public class MachineIssueViewModel
    {
        public Guid IssueId { get; set; }
        public int IssueNo { get; set; }
        public string IssueTitle { get; set; }
        public string Description { get; set; }
        public Guid? IssueCategoryId { get; set; }
        public string IssueCategory { get; set; }
        public int IssueTypeId { get; set; }
        public string IssueType { get; set; }
        public bool IsTargetDateExist { get; set; }
        public DateTime? TargetDate { get; set; }
        public int IssueStatusId { get; set; }
        public string IssueStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ResolvedBy { get; set; }
        public string ResolvedByName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }

    public class IssueUpdateRequestViewModel
    {
        public Guid IssueId { get; set; }
        public string IssueTitle { get; set; }
        public string Description { get; set; }
        public Guid? IssueCategoryId { get; set; }
        public int IssueTypeId { get; set; }
        public Guid? MachineId { get; set; }
        public bool IsTargetDateExist { get; set; }
        public DateTime? TargetDate { get; set; }
        public string ResolvedBy { get; set; }
        public string Note { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public int IssueStatusId { get; set; }
        public Guid? RiskAssessmentLogId { get; set; }
    }
    public class IssueListViewModel
    {
        public IEnumerable<IssueResponseViewModel> List { get; set; }
    }

    public class IssueFileListViewModel
    {
        public List<IssueFileViewModel> issueFiles { get; set; } = new List<IssueFileViewModel>();
    }

    public class IssueDetailViewModel
    {
        public List<IssueFileViewModel> issueFiles { get; set; } = new List<IssueFileViewModel>();
        //public List<IssueCommentResponseViewModel> Comments { get; set; } = new List<IssueCommentResponseViewModel>();
        public IssueResponseViewModel Details { get; set; }
    }
    public class IssueFileViewModel
    {
        public Guid IssueFileId { get; set; }
        public Guid IssueId { get; set; }
        public string FileUniqueName { get; set; }
        public string FileURL { get; set; }
    }

    public class SearchIssueRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
        public Guid? IssueCategoryId { get; set; }
        public int? IssueStatusId { get; set; }
        public int? IssueTypeId { get; set; }
        public Guid? MachineId { get; set; }

    }

    public class IssueSearchResponseViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<IssueResponseViewModel> List { get; set; }
    }

    public class ExistingIssuesResponseViewModel
    {
        public Guid IssueId { get; set; }
        public string IssueTitle { get; set; }
    }

    public class IssueCommentRequestViewModel
    {
        public Guid IssueId { get; set; }
        public string Comment { get; set; }
    }
    public class IssueCommentResponseViewModel : IssueCommentRequestViewModel
    {
        public Guid IssueCommentId { get; set; }
        public string IssueName { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
