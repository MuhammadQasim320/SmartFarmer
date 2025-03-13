namespace SmartFarmer.Domain.Model
{
    public class Event
    {
        public Guid EventId { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }
        public bool ShowWebPopup { get; set; }
        public bool ShowAppPopup { get; set; }

        //FK
        public int EventTypeId { get; set; }
        public string CreatedBy { get; set; }
        public Guid? MachineId { get; set; }
        public EventType EventType { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Machine Machine { get; set; }
        public Guid? CheckListMachineMappingId { get; set; }
        public CheckListMachineMapping CheckListMachineMapping { get; set; }
    }
}
