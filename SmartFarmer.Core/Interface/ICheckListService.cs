using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Core.Interface
{
    public interface ICheckListService
    {
        CheckListViewModel AddCheckList(string CreatedBy,CheckListRequestViewModel model);
        StartCheckListResponseViewModel StartCheckList(string OperatorId, StartCheckListViewModel model,string UserMasterAdminId);
        CheckListViewModel GetCheckListDetails(Guid checkListId);
        CheckListViewModel UpdateCheckListDetails(CheckListViewModel model);
        OperatorCheckListResponseViewModel UpdateCheckListDetailByOperatorId(OperatorCheckListResponseViewModel model);
        CheckListListViewModel GetCheckListList(string UserMasterAdminId);
        CheckListSearchResponseViewModel GetCheckListListBySearchWithPagination(string UserMasterAdminId,CheckListSearchRequestViewModel model);
        
        //CheckListItem functions.
        GetCheckListItemsViewModel AddCheckListItems(Guid checkListId, List<CheckListItemsListViewModel> model);
        CheckListItemListViewModel GetCheckListItems(Guid checkListId);
        StartCheckListResponseViewModel GetLastCheckList(Guid checkListId, Guid machineId);
        PreCheckLogsResponseViewModel GetPreCheckLogsBySearch(string UserMasterAdminId, PreCheckLogsRequestViewModel model);
        bool IsCheckListExist(Guid checkListId);
        bool IsCheckListItemExist(Guid checkListItemId);
        bool DeleteCheckList(Guid checkListId);
        bool IsCheckListItemExists(Guid checkListId, Guid checkListItemId);
        MachineCheckListListViewModel GetCheckListList(Guid machineId ,string UserMasterAdminId);
        bool DeleteCheckListItem(Guid checkListItemId);
    }
}
