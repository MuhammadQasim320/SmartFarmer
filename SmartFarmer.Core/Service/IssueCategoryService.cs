using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;

namespace SmartFarmer.Core.Service
{
    public class IssueCategoryService : IIssueCategoryService
    {
        private readonly IIssueCategoryRepository _issueCategoryRepository;
        public IssueCategoryService(IIssueCategoryRepository issueCategoryRepository)
        {
            _issueCategoryRepository = issueCategoryRepository;
        }

        /// <summary>
        /// add issueCategory into system
        /// </summary>
        /// <param name="issueCategory"></param>
        /// <returns></returns>
        public IssueCategoryResponseViewModel AddIssueCategory(string CreatedBy, IssueCategoryRequestViewModel issueCategory)
        {
            return Mapper.MapIssueCategoryEntityToIssueCategoryResponseViewModel(_issueCategoryRepository.AddIssueCategory(Mapper.MapIssueCategoryRequestViewModelToIssueCategoryEntity(CreatedBy, issueCategory)));
        }

        /// <summary>
        /// get issueCategory by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IssueCategoryCountRequestViewModel GetIssueCategoryListBySearchWithPagination(SearchIssueCategoryRequestViewModel model, string masterAdminId)
        {
            IssueCategoryCountRequestViewModel issueCategoryList = new IssueCategoryCountRequestViewModel();
            issueCategoryList.List = _issueCategoryRepository.GetIssueCategoryListBySearch(model.PageNumber, model.PageSize, model.SearchKey, masterAdminId).Select(a => Mapper.MapIssueCategoryEntityToIssueCategoryResponseViewModel(a)).ToList();
            issueCategoryList.TotalCount = _issueCategoryRepository.GetIssueCategoryCountBySearch(model.SearchKey, masterAdminId);
            return issueCategoryList;
        }

        /// <summary>
        /// check issue category existence
        /// </summary>
        /// <param name="issueCategoryId"></param>
        /// <returns></returns>
        public bool IsIssueCategoryExist(Guid issueCategoryId)
        {
            return _issueCategoryRepository.IsIssueCategoryExist(issueCategoryId);
        }

        /// <summary>
        /// get issue category details
        /// </summary>
        /// <param name="issueCategoryId"></param>
        /// <returns></returns>
        public IssueCategoryResponseViewModel GetIssueCategoryDetails(Guid issueCategoryId)
        {
            return Mapper.MapIssueCategoryEntityToIssueCategoryResponseViewModel(_issueCategoryRepository.GetIssueCategoryDetails(issueCategoryId));
        }

        /// <summary>
        ///update issue category details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IssueCategoryResponseViewModel UpdateIssueCategoryDetails(IssueCategoryResponseViewModel model)
        {
            return Mapper.MapIssueCategoryEntityToIssueCategoryResponseViewModel(_issueCategoryRepository.UpdateIssueCategoryDetails(model.IssueCategoryId, model.Category));
        }

        /// <summary>
        /// get IssueCategory name list 
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public IssueCategoryNameListViewModel GetIssueCategoryNameList(string UserMasterAdminId)
        {
            IssueCategoryNameListViewModel machineCategoryNameListViewModel = new();
            machineCategoryNameListViewModel.List = _issueCategoryRepository.GetIssueCategoryNameList(UserMasterAdminId).Select(a => Mapper.MapIssueCategoryEntityToIssueCategoryNameViewModel(a))?.ToList();
            return machineCategoryNameListViewModel;
        }

        /// <summary>
        /// delete IssueCategory 
        /// </summary>
        /// <param name="issueCategoryId"></param>
        /// <returns></returns>
        public bool DeleteIssueCategory(Guid issueCategoryId)
        {
            return _issueCategoryRepository.DeleteIssueCategory(issueCategoryId);
        }
    }
}
