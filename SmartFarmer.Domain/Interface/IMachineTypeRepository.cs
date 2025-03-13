using SmartFarmer.Domain.Model;

namespace SmartFarmer.Domain.Interface
{
    public interface IMachineTypeRepository
    {
        MachineType AddMachineType(MachineType model);
        MachineType GetMachineTypeDetails(Guid machineTypeId);
        MachineType UpdateMachineTypeDetails(MachineType model);
        bool IsMachineTypeExist(Guid machineTypeId);
        bool IsMachineStatusExist(int machineStatusId);
        bool IsUnitsTypeExist(int unitsTypeId);
        IEnumerable<MachineType> GetMachineTypeListBySearch(string masterAdminId,int pageNumber, int pageSize, string searchKey, bool? NeedsTraining, Guid? RiskAssessmentId);
        int GetMachineTypeCountBySearch(string masterAdminId, string searchKey, bool? NeedsTraining, Guid? RiskAssessmentId);
        List<MachineType> GetMachineTypeNameList(string masterAdminId);
    }
}
