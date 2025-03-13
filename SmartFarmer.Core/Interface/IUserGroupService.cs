using SmartFarmer.Core.ViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IUserGroupService
    {
        UserGroupResponseViewModel AddUserGroup(string CreatedBy,UserGroupRequestViewModel model);
        UserGroupCountRequestViewModel GetUserGroupListBySearchWithPagination(string UserMasterAdminId,SearchUserGroupRequestViewModel model);
        bool IsUserGroupExist(Guid userGroupId);
        UserGroupResponseViewModel GetUserGroupDetails(Guid userGroupId);
        UserGroupResponseViewModel UpdateUserGroupDetails(UserGroupResponseViewModel model);
        UserGroupListViewModel GetUserGroupList(string UserMasterAdminId);
        bool DeleteUserGroup(Guid userGroupId);
        ApplicationUserDetailsViewModel GetUserDetails(Guid userGroupId);
        WelfareRoutineResponseViewModel GetWelfareRoutineDetails(Guid userGroupId);

    }
}
