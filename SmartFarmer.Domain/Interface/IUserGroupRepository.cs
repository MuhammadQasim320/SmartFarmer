using SmartFarmer.Domain.Model;

namespace SmartFarmer.Domain.Interface
{
    public interface IUserGroupRepository
    {
        UserGroup AddUserGroup(UserGroup model);
        IEnumerable<UserGroup> GetUserGroupListBySearch(int pageNumber, int pageSize, string searchKey,string UserMasterAdminId);
        int GetUserGroupCountBySearch(string searchKey,string UserMasterAdminId);
        bool IsUserGroupExist(Guid userGroupId);
        UserGroup GetUserGroupDetails(Guid userGroupId);
        UserGroup UpdateUserGroupDetails(Guid userGroupId, UserGroup model);
        IEnumerable<UserGroup> GetUserGroupList(string UserMasterAdminId);
        bool DeleteUserGroup(Guid userGroupId);
        ApplicationUser GetUserDetails(Guid userGroupId);
        WelfareRoutine GetWelfareRoutineDetails(Guid userGroupId);
    }
}
