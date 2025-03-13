using SmartFarmer.Core.ViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IMachineTypeService
    {
        MachineTypeViewModel AddMachineType(string CreatedBy,MachineTypeRequestViewModel model);
        MachineTypeViewModel GetMachineTypeDetails(Guid machineTypeId);
        MachineTypeViewModel UpdateMachineTypeDetails(MachineTypeViewModel model);
        bool IsMachineTypeExist(Guid machineTypeId);
        bool IsMachineStatusExist(int machineStatusId);
        bool IsUnitsTypeExist(int unitsTypeId);
        MachineTypeSearchResponseViewModel GetMachineTypeListBySearchWithPagination(string masterAdminId,MachineTypeSearchRequestViewModel model);
        MachineTypeNameListViewModel GetMachineTypeNameList(string masterAdminId);
    }
}
