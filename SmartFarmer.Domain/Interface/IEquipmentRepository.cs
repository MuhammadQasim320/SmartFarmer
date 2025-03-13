using Microsoft.AspNetCore.Http.HttpResults;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Interface
{
    public interface IEquipmentRepository
    {
        IEnumerable<Machine> GetEquipmentListBySearch(int pageNumber, int pageSize, string searchKey, int? operatorStatusId,bool? HasIssues, bool? isOUtOfSeason, string loginUserMasterAdminId);
        int GetEquipmentCountBySearch(string searchKey, int? operatorStatusId, bool? HasIssues, bool? isOUtOfSeason, string loginUserMasterAdminId);
        IEnumerable<Event> GetEquipmentHistory(Guid machineId, int pageNumber, int pageSize, int? EventTypeId, string UserMasterAdminId);
        int GetEquipmentHistoryCountBySearch(Guid machineId, int? EventTypeId, string UserMasterAdminId);
        IEnumerable<Event> GetEquipmentPreCheckHistory(Guid machineId, string searchKey, Guid? MachineId, int? resultId, string UserMasterAdminId);
        int GetEquipmentPreCheckHistoryCountBySearch(Guid machineId,string searchKey, Guid? MachineId, int? resultId, string UserMasterAdminId);
    }
}
