using SmartFarmer.Domain.Model;

namespace SmartFarmer.Domain.Interface
{
    public interface IIssueCategoryRepository
    {
        IssueCategory AddIssueCategory(IssueCategory issueCategory);
        IEnumerable<IssueCategory> GetIssueCategoryListBySearch(int pageNumber, int pageSize, string searchKey,string masterAdminId);
        int GetIssueCategoryCountBySearch(string searchKey, string masterAdminId);
        bool IsIssueCategoryExist(Guid issueCategoryId);
        IssueCategory GetIssueCategoryDetails(Guid issueCategoryId);
        IssueCategory UpdateIssueCategoryDetails(Guid issueCategoryId,string Category);
        List<IssueCategory> GetIssueCategoryNameList(string UserMasterAdminId);
        bool DeleteIssueCategory(Guid issueCategoryId);
    }
}
