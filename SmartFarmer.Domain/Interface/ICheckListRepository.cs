using SmartFarmer.Domain.Model;

namespace SmartFarmer.Domain.Interface
{
    public interface ICheckListRepository
    {
        CheckList AddCheckList(CheckList model);
        CheckListMachineMapping StartCheckList(CheckListMachineMapping model);
        CheckListMachineMapping GetLastCheckList(Guid checkListId, Guid machineId);
        CheckListMachineMapping GetMachineLastCheckList(Guid machineId);
        CheckList GetCheckListDetails(Guid checkListId);
        CheckList UpdateCheckListDetails(CheckList model);
        CheckList UpdateCheckListDetailByOperatorId(CheckList model);
        bool IsCheckListExist(Guid checkListId);
        bool IsCheckListItemExist(Guid checkListItemId);
        IEnumerable<CheckList> GetCheckListListBySearch(int pageNumber, int pageSize, string searchKey, Guid? MachineTypeId, int? CheckTypeId, int? FrequencyTypeId,string UserMasterAdminId);
        IEnumerable<CheckList> GetCheckListList(string UserMasterAdminId);
        int GetCheckListCountBySearch(string searchKey, Guid? MachineTypeId, int? CheckTypeId, int? FrequencyTypeId,string UserMasterAdminId);
        bool DeleteCheckList(Guid checkListId);

        //CheckListItem functions.
        CheckListItem AddCheckListItems(CheckListItem model);
        CheckListItem UpdateCheckListItems(CheckListItem model);
        IEnumerable<CheckListItem> GetCheckListItems(Guid checkListId);
        bool IsCheckListItemExists(Guid checkListId, Guid checkListItemId);
        IEnumerable<CheckList> GetCheckListOfMachineType(Guid MachineTypeId);
        //CheckList GetCheckListDetailOfMachineType(Guid MachineTypeId);
        IEnumerable<CheckList> GetPreCheckListOfMachineType(Guid MachineTypeId, string searchKey, int? FrequencyTypeId);
        IEnumerable<CheckList> GetPreCheckListOfMachineTypeforDashboard(Guid MachineTypeId);
        CheckListMachineMapping GetMachineCheckListDate(Guid MachineId, Guid CheckListId);
        IEnumerable<CheckListMachineMapping> GetPreCheckLogsBySearchWithPagination(int pageNumber, int pageSize, string searchKey, Guid? MachineId, int? FrequencyTypeId, string UserMasterAdminId);
        int GetPreCheckLogsCountBySearch(string searchKey, Guid? MachineId, int? FrequencyTypeId, string UserMasterAdminId);
        IEnumerable<CheckList> GetCheckListList(Guid machineId,string UserMasterAdminId);
        bool DeleteCheckListItem(Guid checkListItemId);

    }
}
