using SmartFarmer.Core.ViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IIssueCategoryService
    {
        IssueCategoryResponseViewModel AddIssueCategory(string CreatedBy,IssueCategoryRequestViewModel issueCategory);
        IssueCategoryCountRequestViewModel GetIssueCategoryListBySearchWithPagination(SearchIssueCategoryRequestViewModel model,string masterAdminId);
        bool IsIssueCategoryExist(Guid issueCategoryId);
        IssueCategoryResponseViewModel GetIssueCategoryDetails(Guid issueCategoryId);
        IssueCategoryResponseViewModel UpdateIssueCategoryDetails(IssueCategoryResponseViewModel model);
        IssueCategoryNameListViewModel GetIssueCategoryNameList(string UserMasterAdminId);
        bool DeleteIssueCategory(Guid issueCategoryId);
    }
}
