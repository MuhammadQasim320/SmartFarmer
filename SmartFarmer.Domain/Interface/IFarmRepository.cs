using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Interface
{
    public interface IFarmRepository
    {
        Farm AddFarm(Farm field);
        Farm GetFarmDetail(Guid farmId);
        IEnumerable<Farm> GetFarmListBySearch(string searchKey,string email);
        bool IsFarmExist(Guid fieldId);
        bool IsFarmAssignedToUser(Guid farmId, string email);
        //bool AssignfarmToOperator(Guid farmId, string UserId);
        bool SwitchFarm(Guid farmId, string email);
        bool IsAlarmActionExist(Guid alarmActionId);
        AlarmAction AddAlarmAction(AlarmAction model);
        AlarmAction UpdateAlarmAction(Guid alarmActionId, AlarmAction model);
        AlarmAction GetAlarmActionDetails(string MasterUserId);
        Guid GetLoginUserFarmId(string MasterUserId);
        IEnumerable<Farm> GetFarmListBySearchWithPagiation(int pageNumber, int pageSize, string searchKey);
        int GetFarmCountBySearch(string searchKey);
        Farm UpdateFarmDetail(Guid farmId, string name,string masterAdminFirstName, string masterAdminLastName);
        List<Farm> GetAllFarmList();
        bool AccessFarm(Guid farmId, string email);
        bool IsFarmAssigned(Guid farmId, string email);
    }
}
