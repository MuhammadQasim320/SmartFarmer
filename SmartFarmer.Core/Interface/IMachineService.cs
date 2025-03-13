using SmartFarmer.Core.ViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IMachineService
    {
        MachineResponseViewModel AddMachine(string createdBy, MachineRequestViewModel machine);
        MachineCountRequestViewModel GetMachineListBySearchWithPagination(bool? Find, string LogInUserId,string MasterAdminId,SearchMachineRequestViewModel model);
        RecentMachineCountRequestViewModel GetRecentMachineDetails(string userId,string UserMasterAdminId, SearchMachineRequestViewModel model);
        ActiveMachineResponseViewModel GetActiveMachineDetails(string userId);
        bool IsMachineExist(Guid machineId);
        MachineResponseWithDueCheckViewModel GetMachineDetails(string LogInUser,Guid machineId, string UserMasterAdminId);
        MachineResponseViewModel UpdateMachineDetails(MachineResponseViewModel model);
        MachineResponseViewModel UpdateMachineWorkingDetails(Guid machineId, string WorkingIn);
        FileViewModel GetMachineImageFile(Guid machineId);
        FileViewModel GetMachineQRFile(Guid machineId);
        bool UpdateMachineImageFile(Guid machineId, string fileName, string fileLink);
        bool UpdateQRFile(Guid machineId, string fileName, string fileLink, long? machineCode);
        MachineNameListViewModel GetMachineNameList(string masterId);
        bool UpdateMachineStatus(string operatorId, Guid machineId, string ReasonOfServiceRemoval,int machineStatusId);
        bool StartOperating(Guid machineId, string operatorId, string location,string masterAdminId);
        bool StopOperating(Guid machineId, string operatorId, string location);
        IEnumerable<MachineResponseWithDueCheckViewModel> GetMachineDetailSearch(string SearchKey, string UserMasterAdminId);
        int UpdateMachineResultUnsafe(string masterAdminId);
        int GetOperatorActiveMachineCounts(string userId);
    }
}
