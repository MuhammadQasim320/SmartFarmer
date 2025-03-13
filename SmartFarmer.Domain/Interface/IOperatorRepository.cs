using SmartFarmer.Domain.Model;
using System.Reflection;

namespace SmartFarmer.Domain.Interface
{
    public interface IOperatorRepository
    {
        IEnumerable<ApplicationUser> GetOperatorListBySearch(int pageNumber, int pageSize, string searchKey, int? operatorStatusId, Guid? userGroupId,string UserMasterAdminId);
        int GetOperatorCountBySearch(string searchKey, int? operatorStatusId, Guid? userGroupId,string UserMasterAdminId);
        bool IsOperatorStatusExist(int operatorStatusId);
        void  AddNotification(string userId, string title ,string description);
        IEnumerable<Notification> GetAllNotification(string userId);
    }
}
