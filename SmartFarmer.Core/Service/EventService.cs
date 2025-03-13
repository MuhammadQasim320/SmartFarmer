using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;

namespace SmartFarmer.Core.Service
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        /// <summary>
        /// add event into system
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public EventResponseViewModel AddEvent(int EventTypeId, string CreatedBy, EventRequestViewModel model)
        {
            return Mapper.MapEventEntityToEventResponseViewModel(_eventRepository.AddEvent(Mapper.MapEventRequestViewModelToEventEntity(EventTypeId,CreatedBy, model)));
        }

        /// <summary>
        /// get event by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public EventCountRequestViewModel GetEventListBySearchWithPagination(string UserMasterAdminId, SearchEventRequestViewModel model)
        {
            EventCountRequestViewModel eventList = new EventCountRequestViewModel();
            eventList.List = _eventRepository.GetEventListBySearch(model.PageNumber, model.PageSize, model.SearchKey, model.EventTypeId, model.StartDate, model.EndDate, model.Alarm, UserMasterAdminId).Select(a => Mapper.MapEventEntityToEventResponseViewModel(a)).ToList();
            eventList.TotalCount = _eventRepository.GetEventCountBySearch(model.SearchKey, model.EventTypeId, model.StartDate, model.EndDate, model.Alarm, UserMasterAdminId);
            return eventList;
        }

        /// <summary>
        /// check event existence
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public bool IsEventExist(Guid eventId)
        {
            return _eventRepository.IsEventExist(eventId);
        }

        /// <summary>
        /// get event details
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public EventResponseViewModel GetEventDetails(Guid eventId)
        {
            return Mapper.MapEventEntityToEventResponseViewModel(_eventRepository.GetEventDetails(eventId));
        }

        /// <summary>
        ///update issue category details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public EventResponseViewModel UpdateEventDetails(EventResponseViewModel model)
        {
            return Mapper.MapEventEntityToEventResponseViewModel(_eventRepository.UpdateEventDetails(model.EventId, Mapper.MapEventResponseViewModelToEventEntity(model)));
        }
    }
}
