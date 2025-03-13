using static SmartFarmer.Core.ViewModel.MachinePreCheckHistoryViewModel;

namespace SmartFarmer.Core.ViewModel
{
    public class EventResponseViewModel : EventRequestViewModel
    {
        public Guid EventId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int EventTypeId { get; set; }
        public string EventType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public Guid? MachineId { get; set; }
        public string MachineName { get; set; }
        public string Message { get; set; }
        public bool ShowWebPopup { get; set; }
        public int? ResultId { get; set; }
        public string Result { get; set; }
    }

    public class EventRequestViewModel
    {
        public string Location { get; set; }
    }

    public class MachineHistoryViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<EventResponseViewModel> List { get; set; }
    }

    public class EventFallResponseViewModel : EventRequestViewModel
    {
        public Guid EventId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int EventTypeId { get; set; }
        public string EventType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string Message { get; set; }
        public bool FallDetectionTriggered { get; set; }
        public bool ShowWebPopup { get; set; }
    }

    public class SOSResponseViewModel : EventRequestViewModel
    {
        public Guid EventId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int EventTypeId { get; set; }
        public string EventType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public Guid? MachineId { get; set; }
        public string MachineName { get; set; }
        public string Message { get; set; }
        public bool ShowWebPopup { get; set; }
    }





    public class SearchEventRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
        public int? EventTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Alarm { get; set; }
    }

    public class EventCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<EventResponseViewModel> List { get; set; }
    }

    public class LastEventViewModel
    {
        public Guid? LastEventId { get; set; }
        public string LastEventName { get; set; }
        public DateTime? EventTime { get; set; }
        public string EventLocation { get; set; }
        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; }
    }

    public class MachinePreCheckHistoryViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<PreCheckEventResponseViewModel> List { get; set; }

        public class PreCheckEventResponseViewModel 
        {
            public Guid EventId { get; set; }
            public DateTime CreatedDate { get; set; }
            public int EventTypeId { get; set; }
            public string EventType { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedByName { get; set; }
            public Guid? MachineId { get; set; }
            public string MachineName { get; set; }
            public string Message { get; set; }
            public bool ShowWebPopup { get; set; }
            public string Location { get; set; }
            public int? ResultId { get; set; }
            public string Result { get; set; }
        }
    }
}
