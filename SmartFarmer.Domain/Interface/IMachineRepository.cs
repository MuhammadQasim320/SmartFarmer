using SmartFarmer.Domain.Model;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace SmartFarmer.Domain.Interface
{
    public interface IMachineRepository
    {
        Model.Machine AddMachine(Model.Machine Machine);
        IEnumerable<Model.Machine> GetMachineListBySearch(bool? Find, string LogInUserId,string MasterAdminId, int pageNumber, int pageSize, string searchKey, int? MachineStatusId, Guid? MachineCategoryId, Guid?  MachineTypeId, bool? Archived);
        IEnumerable<Model.Machine> GetMachineListForCheck(string MasterAdminId);
        int GetMachineCountBySearch(bool? Find, string LogInUserId, string MasterAdminId, string searchKey,int? MachineStatusId, Guid? MachineCategoryId, Guid? MachineTypeId, bool? Archived);
        IEnumerable<Model.Machine> GetMachineListBySearch(string MasterAdminId, string searchKey, Guid? MachineId);
        //int GetMachineCountBySearch(string MasterAdminId, string searchKey,int? FrequencyTypeId, Guid? MachineId);
        IEnumerable<Model.Machine> GetRecentMachineDetails(string userId, int pageNumber, int pageSize, string searchKey, Guid? MachineTypeId, Guid? MachineCategoryId );
        IEnumerable<Model.Machine> GetActiceMachineDetails(string userId);
        int GetRecentMachineCountBySearch(string userId, string searchKey, Guid? MachineTypeId, Guid? MachineCategoryId);
        bool IsMachineExist(Guid machineId);
        Model.Machine GetMachineDetails(Guid machineId);
        Model.Machine UpdateMachineDetails(Model.Machine Machine);
        Model.Machine UpdateMachineWorkingDetails(Guid machineId, string WorkingIn);
        Model.Machine GetMachineQRFile(Guid machineId);
        Model.Machine GetMachineImageFile(Guid machineId);
        bool UpdateQRFile(Guid machineId, string fileName, string fileLink, long? machineCode);
        bool UpdateMachineImageFile(Guid machineId, string fileName, string fileLink);
        List<Model.Machine> GetMachineNameList(string masterId);
        List<Model.Machine> GetUserOperating(string userId);
        bool UpdateMachineStatus(string operatorId, Guid machineId, string ReasonOfServiceRemoval, int machineStatusId);
        bool UpdateMachineOperator(string operatorId, Guid machineId,string location);
        bool UpdateMachineOperatormapping(string operatorId, Guid machineId);
        int GetMachineIssuesCount(Guid machineId,string loginUserMasterAdminId);
        bool AssignMachineToOperator(MachineOperatorMapping model);
        MachineOperatorMapping GetMachineLastOperator(Guid machineId);
        bool IsOperatorCanOperate(string operatorId);
        bool IsMachineAlradyAssignedtoThisUser(Guid machineId, string operatorId);
        bool StopOperating(Guid machineId, string operatorId, string location);
        Guid GetMachineType(Guid machineId);
        IEnumerable<Model.Machine> GetMachineDetailSearch(string SearchKey, string UserMasterAdminId);
        MachineType GetMachineTypeDetails(Guid machineTypeId);
        bool CheckTrainingOperatorMapping(string userId, Guid trainingId);
        bool UpdateMachine(Model.Machine machine);




    }
}
