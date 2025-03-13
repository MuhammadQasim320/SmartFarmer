using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Model;
using System.Reflection;

namespace SmartFarmer.Core.Interface
{
    public interface IUserService
    {
        bool IsUserExist(string userId);
        bool IsUserTypeExist(int userTypeId);
        bool IsUserStatusExist(int userStatusId);
        bool IsOperatorExist(string userId);
        string GetUserRole(string userId);
        bool CheckUserEmailExistence(string email);
        bool CheckFarmUserNameExist(string email,Guid farmId);
        bool CheckUserExistsInTheSameFarm(string email, string masterAdminId);
        UserRequestViewModel GetUserListBySearchWithPagination(SearchUserRequestViewModel model,string masterAdminId);
        //UserWelfareChcekViewModel CheckWelfareAlarm(string masterAdminId);
        bool CheckWelfareForApp(string userId);
        bool CreateWelfareForApp(string masterAdminId);
        bool CheckWelfareAlarm(string masterAdminId);
        WelfareAlarmListViewModel CheckWelfareForWeb(string masterAdminId);
        bool CancleWelfareForWeb(string masterAdminId);
        bool CancleWelfareFromWeb(Guid eventId,string loginUserId);
        bool CancleWelfareForApp(string userId);
        ApplicationUserDetailsViewModel GetUserDetails(string userId);
        GetOperatorCheckInDetailsViewModel GetUserChcekInDetails(string userId);
        ApplicationUserViewModel UpdateUserDetails(ApplicationUserRequestViewModel model);
        ApplicationUserNameListViewModel GetUserNameList(string UserMasterAdminId, string searchKey);
        List<GetOperatorBothUserDetailsViewModel> GetOperatorBothUserDetails(string masterAdminId);
        ApplicationUserProfileImageViewModel UpdateProfileImage(ApplicationUserProfileImageViewModel model);
        FileViewModel GetProfileImage(string userId);
        bool UpdateUserLocation(string userId, string location);
        string GetMasterAdminId(string userId);
        UpdateUserResponseViewModel UpdateUserDetailApp(string userId, UpdateUserRequestViewModel model);
        List<string> GetSystemMasterAdminIds();
        ApplicationUserViewModel GetUserDetailByEmail(string email);
        ApplicationUserViewModel GetExistingUserDetailByEmail(string email, string LoginUserMasterAdminId);
        ApplicationUsersListViewModel GetUsersList(string UserMasterAdminId, Guid trainingRecordId);
        List<ApplicationUser> GetUsersListByEmail(string email);
        string GetFarmNameByMasterAdminId(string masterAdminId);
        Farm GetFarmByMasterAdminId(string masterAdminId);

    }
}
