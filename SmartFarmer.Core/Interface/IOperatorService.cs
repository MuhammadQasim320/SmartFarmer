using static SmartFarmer.Core.ViewModel.OperatorViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IOperatorService
    {
        OperatorListViewModel GetOperatorListBySearchWithPagination(string UserMasterAdminId,SearchOperatorRequestViewModel model);
        bool IsOperatorStatusExist(int operatorStatusId);
        IEnumerable<NotificationResponseViewModel> GetAllNotification(string userId);
    }
}
