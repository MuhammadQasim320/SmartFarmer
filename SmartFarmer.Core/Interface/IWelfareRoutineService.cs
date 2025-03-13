using SmartFarmer.Core.ViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IWelfareRoutineService
    {
        bool IsWelfareRoutineExists(Guid welfareRoutineId);
        WelfareRoutineResponseViewModel AddWelfareRoutine(string CreatedBy,WelfareRoutineRequestViewModel model);
        WelfareRoutineCountRequestViewModel GetWelfareRoutineListBySearchWithPagination(string UserMasterAdminId,SearchWelfareRoutineRequestViewModel model);
        WelfareRoutineResponseViewModel GetWelfareRoutineDetails(Guid welfareRoutineId);
        WelfareRoutineResponseViewModel UpdateWelfareRoutineDetail(WelfareRoutineResponseViewModel model);
        bool IsGroupAssignedToOtherWelfareRoutine(Guid userGroupId);
        bool IsGroupAssignedToWelfareRoutine(Guid userGroupId,Guid welfareRoutineId);
        bool IsGroupAssigned(Guid userGroupId,Guid welfareRoutineId);
    }
}
