using SmartFarmer.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartFarmer.Core.ViewModel.SearchEquipmentHistoryRequestViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IEquipmentService
    {
        EquipmentListViewModel GetEquipmentListBySearchWithPagination(SearchEquipmentRequestViewModel model, string loginUserMasterAdminId);
        MachineHistoryViewModel GetEquipmentHistory(Guid machineId, SearchEquipmentHistoryRequestViewModel model, string UserMasterAdminId);
        MachinePreCheckHistoryViewModel GetEquipmentPreCheckHistory(SearchEquipmentPreCheckHistoryRequestViewModel model, string UserMasterAdminId);
    }
}
