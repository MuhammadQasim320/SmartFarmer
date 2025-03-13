using SmartFarmer.Core.ViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IEventService
    {
        EventResponseViewModel AddEvent(int EventTypeId,string CreatedBy,EventRequestViewModel model);
        EventCountRequestViewModel GetEventListBySearchWithPagination(string UserMasterAdminId,SearchEventRequestViewModel model);
        bool IsEventExist(Guid eventId);
        EventResponseViewModel GetEventDetails(Guid eventId);
        EventResponseViewModel UpdateEventDetails(EventResponseViewModel model);
    }
}
