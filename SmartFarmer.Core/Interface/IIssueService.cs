using SmartFarmer.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartFarmer.Core.ViewModel.IssueViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IIssueService
    {
        IssueResponseViewModel AddIssue(string CreatedBy, IssueRequestViewModel model);
        bool IsIssueExist(Guid issueId);
        bool IsIssueFileExist(Guid issueFileId);
        IssueDetailViewModel GetIssueDetails(Guid issueId);
        IssueResponseViewModel UpdateIssueDetails(IssueResponseViewModel issueResponseViewModel);
        IssueListViewModel GetMachineIssuesList(Guid machineId,string masterAdminId);
        IssueFileViewModel UploadIssueFile(IssueFileViewModel model);
        IssueFileListViewModel GetIssueFilesList(Guid issueId);
        IssueSearchResponseViewModel GetIssueListBySearchWithPagination(SearchIssueRequestViewModel model, string masterAdminId);
        List<ExistingIssuesResponseViewModel> GetMachineExistingIssuesList(Guid MachineId);
        //IssueCommentResponseViewModel AddIssueComment(string CreatedBy, IssueCommentRequestViewModel model);
        bool AddIssueComment(IssueCommentRequestViewModel model);
        IssueFileViewModel GetIssueFile(Guid issueFileId);
        bool DeleteIssueFile(Guid issueFileId);
    }
}
