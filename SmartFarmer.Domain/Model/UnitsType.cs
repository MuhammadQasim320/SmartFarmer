namespace SmartFarmer.Domain.Model
{
    public class UnitsType
    {
        public int UnitsTypeId { get; set; }
        public string Units { get; set; }

        //FK
        public ICollection<MachineType> MachineTypes { get; set; }
    }
}
