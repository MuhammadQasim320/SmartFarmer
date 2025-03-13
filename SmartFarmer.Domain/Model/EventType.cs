namespace SmartFarmer.Domain.Model
{
    public class EventType
    {
        public int EventTypeId { get; set; }
        public string Type { get; set; }

        //FK
        public ICollection<Event> Events { get; set; }
    }
}
