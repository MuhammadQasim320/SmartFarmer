using Microsoft.EntityFrameworkCore;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Data.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly SmartFarmerContext _context;
        public EventRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add event into system
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public Event AddEvent(Event Event)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(a=>a.Id == Event.CreatedBy);
            if (Event.Location != null)
            {
                user.Location = Event.Location;
                _context.ApplicationUsers.Update(user);
            }
            var machine = _context.Machines.FirstOrDefault(a => a.MachineId == Event.MachineId);
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In)
            {
                Event.Message = user.FirstName + " " + user.LastName + " clocked in at " + Event.CreatedDate.ToString("hh:mm tt");
                user.OperatorStatusId = (int)Core.Common.Enums.OperatorStatusEnum.Working;
                _context.ApplicationUsers.Update(user);
            }
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Operate)
            {
                Event.Message = user.FirstName + " " + user.LastName + " is operating " + machine.Name;
            }
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Pre_Check)
            {
                Event.Message = user.FirstName + " " + user.LastName + " completed Pre-Check at " + Event.CreatedDate.ToString("hh:mm tt");
            }
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Idle)
            {
                Event.Message = user.FirstName + " " + user.LastName + " left " + machine.Name + " idle";
            }
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Check_In)
            {
                Event.Message = user.FirstName + " " + user.LastName + " checked in ";
            }
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.SOS)
            {
                Event.Message = user.FirstName + " " + user.LastName + " triggered an SOS ";
                Event.ShowWebPopup = true;
            }
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Fall)
            {
                Event.Message = user.FirstName + " " + user.LastName + " may have had a FALL ";
                Event.ShowWebPopup = true;
            }
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Welfare)
            {
                Event.Message = user.FirstName + " " + user.LastName + " missed a WELFARE check ";
                Event.ShowWebPopup = true;
            }
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out)
            {
                Event.Message = user.FirstName + " " + user.LastName + " Clocked Out at " + Event.CreatedDate.ToString("hh:mm tt");
                user.OperatorStatusId = (int)Core.Common.Enums.OperatorStatusEnum.Idle;
                _context.ApplicationUsers.Update(user);
            }
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Service)
            {
                Event.Message = user.FirstName + " " + user.LastName + " completed Service at" + Event.CreatedDate.ToString("hh:mm tt");
            }   
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Cancelled)
            {
                if (string.IsNullOrEmpty(Event.Message))
                {
                    Event.Message=Event.Message;

                }
                
            }
            _context.Events.Add(Event);
            _context.SaveChanges();
            var response = _context.Events.Where(a => a.EventId == Event.EventId).Include(a=>a.EventType).Include(a => a.ApplicationUser).Include(a => a.Machine).FirstOrDefault();
            return Event;
        }
        
        /// <summary>
        /// add check event into system
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public Event AddCheckEvent(Event Event, Guid checkListMachineMappingId)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(a=>a.Id == Event.CreatedBy);
            if (Event.Location != null)
            {
                user.Location = Event.Location;
                _context.ApplicationUsers.Update(user);
            }
            var machine = _context.Machines.FirstOrDefault(a => a.MachineId == Event.MachineId);
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Pre_Check)
            {
                Event.Message = user.FirstName + " " + user.LastName + " completed Pre-Check at " + Event.CreatedDate.ToString("hh:mm tt");
            }
            if (Event.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Service)
            {
                Event.Message = user.FirstName + " " + user.LastName + " completed Service at" + Event.CreatedDate.ToString("hh:mm tt");
            }
            Event.CheckListMachineMappingId = checkListMachineMappingId;
            _context.Events.Add(Event);
            _context.SaveChanges();
            var response = _context.Events.Where(a => a.EventId == Event.EventId).Include(a=>a.EventType).Include(a => a.ApplicationUser).Include(a => a.Machine).FirstOrDefault();
            return Event;
        }

        /// <summary>
        /// get event list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Event> GetEventListBySearch(int pageNumber, int pageSize, string searchKey, int? eventTypeId, DateTime? StartDate, DateTime? EndDate, bool Alarm, string UserMasterAdminId)
        {
            var events = _context.Events.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).Include(a => a.Machine).Include(a => a.ApplicationUser).Include(a => a.EventType).AsQueryable();
            //if (searchKey != null)
            //{
            //    searchKey = searchKey.ToLower();
            //    events = events.Where(a => a.ApplicationUser.FirstName.ToLower().Contains(searchKey) || a.ApplicationUser.LastName.ToLower().Contains(searchKey) || a.Machine.Name.ToLower().Contains(searchKey));
            //}
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                // applicationUsers = applicationUsers.Where(a => a.FirstName.ToLower().Contains(searchKey) || (a.LastName.ToLower().Contains(searchKey)));

                var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                events = events.Where(a => a.Machine.Name.ToLower().Contains(searchKey)||
                    searchWords.Any(word => a.ApplicationUser.FirstName.ToLower().Contains(word) || a.ApplicationUser.LastName.ToLower().Contains(word))
                );
            }


            if (eventTypeId != null)
            {
                events = events.Where(a => a.EventTypeId == eventTypeId);
            }
            if (Alarm != false)
            {
                events = events.Where(a => a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.SOS || a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Fall || a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Welfare);
            }
            //if (StartDate != null && EndDate != null)
            //{
            //    events = events.Where(a => a.CreatedDate >= StartDate && a.CreatedDate <= EndDate);
            //}
            if (StartDate != null)
            {
                events = events.Where(a => a.CreatedDate.Date >= StartDate);
            }
            if (EndDate != null)
            {
                events = events.Where(a => a.CreatedDate.Date <= EndDate);
            }



            return events.OrderByDescending(a => a.CreatedDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        /// <summary>
        /// get events
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Event> GetEvents( string UserId)
        {
            var events = _context.Events.Where(a => a.CreatedBy == UserId ).Include(a => a.Machine).Include(a => a.ApplicationUser).Include(a => a.EventType).AsQueryable();
            
            return events.OrderByDescending(a => a.CreatedDate).ToList();
        }
        /// <summary>
        /// get event count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetEventCountBySearch(string searchKey, int? eventTypeId, DateTime? StartDate, DateTime? EndDate, bool Alarm, string UserMasterAdminId)
        {
            var events = _context.Events.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).Include(a => a.Machine).Include(a => a.ApplicationUser).Include(a => a.EventType).AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                // applicationUsers = applicationUsers.Where(a => a.FirstName.ToLower().Contains(searchKey) || (a.LastName.ToLower().Contains(searchKey)));

                var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                events = events.Where(a => a.Machine.Name.ToLower().Contains(searchKey) ||
                    searchWords.Any(word => a.ApplicationUser.FirstName.ToLower().Contains(word) || a.ApplicationUser.LastName.ToLower().Contains(word))
                );
            }
            if (eventTypeId != null)
            {
                events = events.Where(a => a.EventTypeId == eventTypeId);
            }
            if (Alarm != false)
            {
                events = events.Where(a => a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.SOS || a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Fall || a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Welfare);
            }
            if (StartDate != null && EndDate != null)
            {
                events = events.Where(a => a.CreatedDate >= StartDate && a.CreatedDate <= EndDate);
            }
            else if (StartDate != null)
            {
                events = events.Where(a => a.CreatedDate >= StartDate);
            }
            else if (EndDate != null)
            {
                events = events.Where(a => a.CreatedDate <= EndDate);
            }

            return events.Count();
        }

        /// <summary>
        /// check event existence
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public bool IsEventExist(Guid eventId)
        {
            return _context.Events.Find(eventId) == null ? false : true;
        }

        /// <summary>
        /// get event details
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public Event GetEventDetails(Guid eventId)
        {
            return _context.Events.Where(a=>a.EventId==eventId).Include(a => a.EventType).Include(a => a.ApplicationUser).Include(a => a.Machine).FirstOrDefault();
        }

        /// <summary>
        ///update event details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Event UpdateEventDetails(Guid eventId, Event Event)
        {
            var res = _context.Events.Find(eventId);
            if (res != null)
            {
                res.Location = Event.Location;
                res.Message = Event.Message;
                res.MachineId = Event.MachineId;
                res.EventTypeId = Event.EventTypeId;
                _context.Events.Update(res);
                _context.SaveChanges();
                var result = _context.Events.Where(a => a.EventId == eventId).Include(a => a.EventType).Include(a => a.ApplicationUser).Include(a => a.Machine).FirstOrDefault();
                return result;
            }
            return null;
        }
        /// <summary>
        ///update event details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateEventForWeb(Guid eventId, bool webPopUp)
        {
            var res = _context.Events.Find(eventId);
            if (res != null)
            {
                res.ShowWebPopup = webPopUp;
                _context.Events.Update(res);
                _context.SaveChanges();
                var result = _context.Events.Where(a => a.EventId == eventId).Include(a => a.EventType).Include(a => a.ApplicationUser).Include(a => a.Machine).FirstOrDefault();
                return true;
            }
            return false;
        } 
        /// <summary>
        ///update event details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateEventForApp(Guid eventId, bool AppPopUp)
        {
            var res = _context.Events.Find(eventId);
            if (res != null)
            {
                res.ShowAppPopup = AppPopUp;
                _context.Events.Update(res);
                _context.SaveChanges();
                var result = _context.Events.Where(a => a.EventId == eventId).Include(a => a.EventType).Include(a => a.ApplicationUser).Include(a => a.Machine).FirstOrDefault();
                return true;
            }
            return false;
        }
        
        /// <summary>
        ///Get last event
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Event GetLastEvent(string userId)
        {
            var res = _context.Events.Where(a => a.CreatedBy == userId).Include(a=>a.EventType).OrderByDescending(a=>a.CreatedDate).FirstOrDefault();
            if (res != null)
            {
                return res;
            }
            return null;
        }
        
        /// <summary>
        ///Get last event by machine
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Event GetLastEventByMachineId(Guid machineId)
        {
            var res = _context.Events.Where(a => a.MachineId == machineId).OrderByDescending(a=>a.CreatedDate).FirstOrDefault();
            if (res != null)
            {
                return res;
            }
            return null;
        }
        
        /// <summary>
        ///Get recent machine event date
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DateTime? GetRecentMachineEventDate(Guid machineId)
        {
            var res = _context.Events.Where(a => a.MachineId == machineId && a.Machine.MachineStatusId == (int)Core.Common.Enums.MachineStatusEnum.Active && a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Operate).FirstOrDefault();
            if (res != null)
            {
                return res.CreatedDate;
            }
            return null;
        }
    }
}
