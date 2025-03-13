using SmartFarmer.Domain.Model;

namespace SmartFarmer.Domain.Interface
{
    public interface IWelfareRoutineRepository
    {
        bool IsWelfareRoutineExists(Guid welfareRoutineId);
        WelfareRoutine AddWelfareRoutine(WelfareRoutine model);
        IEnumerable<WelfareRoutine> GetWelfareRoutineListBySearch(int pageNumber, int pageSize, string searchKey,string UserMasterAdminId);
        int GetWelfareRoutineCountBySearch(string searchKey,string UserMasterAdminId);
        WelfareRoutine GetWelfareRoutineDetails(Guid welfareRoutineId);
        WelfareRoutine UpdateWelfareRoutineDetail(Guid welfareRoutineId, WelfareRoutine model);
        WelfareRoutine GetWelfareRoutineDetail(Guid? groupId);
        bool IsGroupAssignedToOtherWelfareRoutine(Guid userGroupId);
        bool IsGroupAssignedToWelfareRoutine(Guid userGroupId,Guid welfareRoutineId);
        bool IsGroupAssigned(Guid userGroupId,Guid welfareRoutineId);
    }
}
