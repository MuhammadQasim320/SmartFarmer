using Microsoft.AspNetCore.Cors.Infrastructure;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Interface
{
    public interface IIssueRepository
    {
        Issue AddIssue(Issue Issue);
        bool IsIssueExist(Guid IssueId);
        bool IsIssueFileExist(Guid IssueFileId);
        Issue GetIssueDetails(Guid issueId);
        Issue UpdateIssueDetails(Issue issue);
        List<Issue> GetMachineIssuesList(Guid machineId, string masterAdminId);
        List<Issue> GetMachineIssues(Guid machineId ,string UserMasterAdminId);
        IssueFile UploadIssueFile(IssueFile issueFile);
        List<IssueFile> GetIssueFilesList(Guid issueId);
        //List<IssueComment> GetIssueCommentList(Guid issueId);
        IEnumerable<Issue> GetIssueListBySearch(int pageNumber, int pageSize, string searchKey, int? issueStatusId, Guid? issueCategoryId, int? issueTypeId, Guid? machineId, string masterAdminId);
        int GetIssueCountBySearch(string searchKey, int? issueStatusId, Guid? issueCategoryId, int? issueTypeId, Guid? MachineId, string masterAdminId);
        List<Issue> GetMachineExistingIssuesList(Guid machineId);
        List<Issue> GetActions(Guid RiskAssessmentLogId, string UserMasterAdminId);
        //IssueComment AddIssueComment(IssueComment IssueComment);
        bool AddIssueComment(Guid IssueId, string Comment);
        IssueFile GetIssueFile(Guid issueFileId);
        bool DeleteIssueFile(Guid issueFileId);
         bool IsIssuesDefected(IEnumerable<Issue> issues);


    }
}
