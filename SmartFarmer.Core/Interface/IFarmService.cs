using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.Interface
{
    public interface IFarmService
    {
        FarmResponseViewModel AddFarm(FarmViewModel farmViewModel);
        FarmCountRequestViewModel GetFarmListBySearch(string LogInUser, string SearchKey,string email);
        bool IsFarmExist(Guid farmId);
        bool IsFarmAssignedToUser(Guid farmId, string email);
        //bool AssignFarm(Guid farmId,string UserId);
        bool SwitchFarm(Guid farmId,string email);
        //FarmResponseViewModel UpdateFarmDetails(FarmResponseViewModel model);
        bool IsAlarmActionExist(Guid alarmActionId);
        AddAlarmActionResponseViewModel AddAlarmAction(string CreatedBy, AddAlarmActionRequestViewModel model,string smsNumbersJson);
        AddAlarmActionResponseViewModel GetAlarmActionDetails(string MasterUserId);
        Guid GetLoginUserFarmId(string MasterUserId);
        CountFarmResponseViewModel GetFarmListBySearchWithPagination(FarmSearchRequestViewModel model);
        FarmDetailViewModel GetFarmDetail(Guid farmId);
        FarmDetailViewModel UpdateFarmDetail(FarmDetailViewModel model);
        FarmNameListViewModel GetAllFarmList();
        bool AccessFarm(Guid farmId, string email);
        bool IsFarmAssigned(Guid farmId, string email);
    }
}
