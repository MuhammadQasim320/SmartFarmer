using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class Issue
    {
        public Guid IssueId { get; set; }
        public int IssueNo { get; set; }
        public string IssueTitle { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public bool IsTargetDateExist { get; set; }
        public DateTime? TargetDate { get; set; }


        //Fk
        public string ResolvedBy { get; set; }
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ApplicationUser Operator { get; set; }
        public int IssueTypeId { get; set; }
        public IssueType IssueType { get; set; }
        public Guid? IssueCategoryId { get; set; }
        public IssueCategory IssueCategory { get; set; }
        public Guid? MachineId { get; set; }
        public Machine Machine { get; set; }
        public Guid? RiskAssessmentLogId { get; set; }
        public RiskAssessmentLog RiskAssessmentLog { get; set; }
        public int IssueStatusId { get; set; }
        public IssueStatus IssueStatus { get; set; }
        public ICollection<IssueFile> IssueFiles { get; set; }
        //public ICollection<IssueComment> IssueComments { get; set; }
        //public int Count { get; set; }
    }
}
