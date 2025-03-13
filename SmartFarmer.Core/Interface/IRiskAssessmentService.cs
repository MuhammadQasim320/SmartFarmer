using SmartFarmer.Core.ViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IRiskAssessmentService
    {
        RiskAssessmentViewModel AddRiskAssessment(string CreatedBy,RiskAssessmentRequestViewModel model);
        RiskAssessmentLogViewModel AddRiskAssessmentLog(string CreatedBy, RiskAssessmentLogRequestViewModel model);
        RiskAssessmentResponseViewModel GetRiskAssessmentDetails(Guid riskAssessmentId);
        RiskAssessmentLogViewModel GetRiskAssessmentLogDetails(Guid riskAssessmentLogId);
        RiskAssessmentViewModel UpdateRiskAssessmentDetails(RiskAssessmentViewModel model);
        RiskAssessmentLogViewModel UpdateRiskAssessmentLogDetails(RiskAssessmentLogViewModel model);
        bool IsRiskAssessmentExist(Guid riskAssessmentId);
        bool IsRiskAssessmentLogExist(Guid riskAssessmentLogId);
        bool IsInitialRiskExist(int InitialRiskId);
        //bool IsActionExist(Guid ActionId);
        RiskAssessmentSearchResponseViewModel GetRiskAssessmentListBySearchWithPagination(string UserMasterAdminId,RiskAssessmentSearchRequestViewModel model);
        RiskAssessmentLogSearchResponseViewModel GetRiskAssessmentLogListBySearchWithPagination(string UserMasterAdminId,RiskAssessmentLogSearchRequestViewModel model);
        RiskAssessmentNameListViewModel GetRiskAssessmentNameList(string createdBy);

        //RiskAssessment files functions.
        RiskAssessmentFileViewModel AddRiskAssessmentFile(RiskAssessmentFileViewModel model);
        IEnumerable<RiskAssessmentFileViewModel> GetRiskAssessmentFiles(Guid riskAssessmentId);
        RiskAssessmentFileViewModel GetRiskAssessmentFile(Guid riskAssessmentFileId);
        bool IsRiskAssessmentFileExist(Guid riskAssessmentFileId);
        RiskAssessmentFileViewModel UploadRiskAssessmentFile(RiskAssessmentFileViewModel model);
        bool DeleteRiskAssessmentFile(Guid riskAssessmentFileId);
        List<IssueResponseViewModel> GetCorrectiveIssueList(Guid riskAssessmentLogId, string UserMasterAdminId);
        RiskAssessmentLogNameListViewModel GetRiskAssessmentLogNameList(string createdBy);

        RiskAssessmentWithLogDetailViewModel GetRiskAssessmentWithLogDetail(Guid riskAssessmentId);

    }
}
