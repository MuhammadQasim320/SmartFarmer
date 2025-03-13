using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;

namespace SmartFarmer.Core.Service
{
    public class UserGroupService : IUserGroupService
    {
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUserRepository _userRepository;
        public UserGroupService(IUserGroupRepository userGroupRepository, IUserRepository userRepository)
        {
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Is UserGroup Exist
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsUserGroupExist(Guid userGroupId)
        {
            return _userGroupRepository.IsUserGroupExist(userGroupId);
        }

        /// <summary>
        /// add userGroup into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserGroupResponseViewModel AddUserGroup(string CreatedBy, UserGroupRequestViewModel model)
        {
            return Mapper.MapUserGroupEntityToUserGroupResponseViewModel(_userGroupRepository.AddUserGroup(Mapper.MapUserGroupRequestViewModelToUserGroupEntity(CreatedBy,model)));
        }

        /// <summary>
        /// get userGroup by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserGroupCountRequestViewModel GetUserGroupListBySearchWithPagination(string UserMasterAdminId, SearchUserGroupRequestViewModel model)
        {
            UserGroupCountRequestViewModel userGroupList = new UserGroupCountRequestViewModel();
            userGroupList.List = _userGroupRepository.GetUserGroupListBySearch(model.PageNumber, model.PageSize, model.SearchKey, UserMasterAdminId).Select(a => Mapper.MapUserGroupEntityToUserGroupResponseViewModel(a)).ToList();
            userGroupList.TotalCount = _userGroupRepository.GetUserGroupCountBySearch(model.SearchKey, UserMasterAdminId);
            return userGroupList;
        }

        /// <summary>
        /// get userGroup deatils
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserGroupResponseViewModel GetUserGroupDetails(Guid userGroupId)
        {
            return Mapper.MapUserGroupEntityToUserGroupResponseViewModel(_userGroupRepository.GetUserGroupDetails(userGroupId));
        }

        /// <summary>
        /// update userGroup details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserGroupResponseViewModel UpdateUserGroupDetails(UserGroupResponseViewModel model)
        {
            return Mapper.MapUserGroupEntityToUserGroupResponseViewModel(_userGroupRepository.UpdateUserGroupDetails(model.UserGroupId, Mapper.MapUserGroupRequestViewModelToUserGroupEntity(model)));
        }

        /// <summary>
        /// Get UserGroup Name List
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public UserGroupListViewModel GetUserGroupList(string UserMasterAdminId)
        {
            UserGroupListViewModel userGroupListViewModel = new();
            userGroupListViewModel.UserGroupList = _userGroupRepository.GetUserGroupList(UserMasterAdminId).Select(a => Mapper.MapUserGroupEntityToUserGroupNameListViewModel(a))?.ToList();
            return userGroupListViewModel;
        }


        /// <summary>
        /// delete userGroup 
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public bool DeleteUserGroup(Guid userGroupId)
        {
            return _userGroupRepository.DeleteUserGroup(userGroupId);
        }



        /// <summary>
        /// get user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApplicationUserDetailsViewModel GetUserDetails(Guid userGroupId)
        {
            ApplicationUserDetailsViewModel applicationUserDetailsView = new ApplicationUserDetailsViewModel();
            applicationUserDetailsView.UserDetails = Mapper.MapApplicationUserEntityToApplicationUserViewModel(_userGroupRepository.GetUserDetails(userGroupId));
            return applicationUserDetailsView;
        }



        /// <summary>
        /// get welfareRoutine 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WelfareRoutineResponseViewModel GetWelfareRoutineDetails(Guid userGroupId)
        {
            return Mapper.MapWelfareRoutineEntityToWelfareRoutineResponseViewModel(_userGroupRepository.GetWelfareRoutineDetails(userGroupId));
        }
    }
}
