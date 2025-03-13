using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class RiskAssessmentLog
    {
        public Guid RiskAssessmentLogId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool Expires { get; set; }
        public int? Validity { get; set; }
        public bool Archived { get; set; }

        //Fk
        public Guid? RiskAssessmentId { get; set; }
        public RiskAssessment RiskAssessment { get; set; }
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //public Guid? ActionId { get; set; }
        //public Action Action { get; set; }
        public int? InitialRiskId { get; set; }
        public InitialRiskAndAdjustedRisk InitialRisk { get; set; }
        public int? AdjustedRiskId { get; set; }
        public InitialRiskAndAdjustedRisk AdjustedRisk { get; set; }
        public ICollection<Issue> Issues { get; set; }
    }
}
