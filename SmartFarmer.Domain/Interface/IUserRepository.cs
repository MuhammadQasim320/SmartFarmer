using SmartFarmer.Domain.Model;
using System.Reflection;

namespace SmartFarmer.Domain.Interface
{
    public interface IUserRepository
    {
        bool IsUserExist(string userId);
        bool IsUserTypeExist(int userTypeId);
        bool IsUserStatusExist(int userStatusId);
        bool IsOperatorExist(string userId);
        string GetUserRole(string userId);
        bool CheckUserEmailExistence(string email);
        bool CheckFarmUserNameExist(string email, Guid farmId);
        bool CheckUserExistsInTheSameFarm(string email, string masterAdminId);
        IEnumerable<ApplicationUser> GetUserListBySearch(int pageNumber, int pageSize, string searchKey,string masterAdminId);
        IEnumerable<ApplicationUser> GetUsers(string masterAdminId);
        int GetUserCountBySearch(string searchKey,string masterAdminId);
        ApplicationUser GetUserDetails(string userId);
        ApplicationUser UpdateUserDetails(ApplicationUser user);
        List<ApplicationUser> GetUserNameList(string UserMasterAdminId, string searchKey);
        List<ApplicationUser> GetOperatorBothUserDetails(string masterAdminId);
        ApplicationUser UpdateProfileImage(ApplicationUser applicationUser);
        ApplicationUser GetProfileImage(string userId);
        bool UpdateUserLocation(string userId, string location);
        bool InTrainingRecordExistance(string userId);
        string GetMasterAdminId(string userId);

        ApplicationUser UpdateUserDetailApp(ApplicationUser model);
        List<string> GetSystemMasterAdminIds();
        AlarmAction GetAlarmActionDetail(string userId);
        ApplicationUser GetUserDetailByEmail(string email);
        ApplicationUser GetExistingUserDetailByEmail(string email, string UserMasterAdminId);
        List<ApplicationUser> GetUsersList(string UserMasterAdminId);
        bool IsUserAddedTrainingRecord(Guid trainingRecordId, string userId);
        List<ApplicationUser> GetUsersListByEmail(string email);
        string GetFarmNameByMasterAdminId(string masterAdminId);
        Farm GetFarmByMasterAdminId(string masterAdminId);
    }
}
