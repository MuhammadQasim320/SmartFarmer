using SmartFarmer.Domain.Model;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace SmartFarmer.Domain.Interface
{
    public interface IEventRepository
    {
        Event AddEvent(Event Event);
        Event AddCheckEvent(Event Event, Guid checkListMachineMappingId);
        IEnumerable<Event> GetEventListBySearch(int pageNumber, int pageSize, string searchKey, int? eventTypeId, DateTime? StartDate, DateTime? EndDate, bool Alarm, string UserMasterAdminId);
        int GetEventCountBySearch(string searchKey, int? eventTypeId, DateTime? StartDate, DateTime? EndDate, bool Alarm, string UserMasterAdminId);
        bool IsEventExist(Guid eventId);
        Event GetEventDetails(Guid eventId);
        Event UpdateEventDetails(Guid eventId, Event Event);
        bool UpdateEventForWeb(Guid eventId,bool webPopUp);
        bool UpdateEventForApp(Guid eventId,bool appPopUp);
        Event GetLastEvent(string userId);
        IEnumerable<Event> GetEvents(string userId);
        Event GetLastEventByMachineId(Guid machineId);
        DateTime? GetRecentMachineEventDate(Guid machineId);
    }
}
