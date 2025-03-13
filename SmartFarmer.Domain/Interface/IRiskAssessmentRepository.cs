using SmartFarmer.Domain.Model;
using System.Reflection;

namespace SmartFarmer.Domain.Interface
{
    public interface IRiskAssessmentRepository
    {
        RiskAssessment AddRiskAssessment(RiskAssessment model);
        RiskAssessmentLog AddRiskAssessmentLog(RiskAssessmentLog model);
        RiskAssessment GetRiskAssessmentDetails(Guid riskAssessmentId);
        RiskAssessmentLog GetRiskAssessmentLogDetails(Guid riskAssessmentLogId);
        RiskAssessment UpdateRiskAssessmentDetails(RiskAssessment model);
        RiskAssessmentLog UpdateRiskAssessmentLogDetails(RiskAssessmentLog model);
        bool IsRiskAssessmentExist(Guid riskAssessmentId);
        bool IsRiskAssessmentLogExist(Guid riskAssessmentLogId);
        bool IsInitialRiskExist(int initialRiskId);
        //bool IsActionExist(Guid actionId);
        IEnumerable<RiskAssessment> GetRiskAssessmentListBySearch(int pageNumber, int pageSize, string searchKey,string UserMasterAdminId);
        IEnumerable<RiskAssessmentLog> GetRiskAssessmentLogListBySearch(int pageNumber, int pageSize, string searchKey,string UserMasterAdminId, bool? Archived, bool? Expires,Guid? RiskAssessmentId);
        int GetRiskAssessmentCountBySearch(string searchKey, string UserMasterAdminId);
        int GetRiskAssessmentLogCountBySearch(string searchKey, string UserMasterAdminId, bool? Archived, bool? Expires, Guid? RiskAssessmentId);
        List<RiskAssessment> GetRiskAssessmentNameList(string createdBy);

        //RiskAssessment file functions
        RiskAssessmentFile AddRiskAssessmentFile(RiskAssessmentFile riskAssessmentFile);
        IEnumerable<RiskAssessmentFile> GetRiskAssessmentFiles(Guid riskAssessmentId);
        RiskAssessmentFile GetRiskAssessmentFile(Guid riskAssessmentFileId);
        bool IsRiskAssessmentFileExist(Guid riskAssessmentFileId);
        RiskAssessmentFile UploadRiskAssessmentFile(RiskAssessmentFile riskAssessmentFile);
        bool DeleteRiskAssessmentFile(Guid riskAssessmentFileId);
        List<Issue> GetCorrectiveIssueList(Guid riskAssessmentLogId, string UserMasterAdminId);
        List<RiskAssessmentLog> GetRiskAssessmentLogNameList(string createdBy);
        IEnumerable<RiskAssessmentFile> GetRiskAssessmentFilesForMachine(Guid? riskAssessmentId);
        RiskAssessmentLog GetRiskAssessmentLogDetailsByRiskAssessment(Guid riskAssessmentId);
    }
}
