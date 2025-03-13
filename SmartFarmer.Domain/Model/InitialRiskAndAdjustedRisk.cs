namespace SmartFarmer.Domain.Model
{
    public class InitialRiskAndAdjustedRisk
    {
        public int InitialRiskAndAdjustedRiskId { get; set; }
        public string RiskValue { get; set; }

        //FK
        public ICollection<RiskAssessmentLog> InitialRiskRiskAssessmentLogs { get; set; }
        public ICollection<RiskAssessmentLog> AdjustedRiskRiskAssessmentLogs { get; set; }
    }
}
