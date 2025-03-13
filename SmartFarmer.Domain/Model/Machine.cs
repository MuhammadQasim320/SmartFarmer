namespace SmartFarmer.Domain.Model
{
    public class Machine
    {
        public Guid MachineId { get; set; }
        public string MachineImage { get; set; }
        public string MachineImageUniqueName { get; set; }
        public string NickName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string QRCode { get; set; }
        public string QRUniqueName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? MOTDate { get; set; }
        public DateTime? LOLERDate { get; set; }
        public int ServiceInterval { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ReasonOfServiceRemoval { get; set; }
        public string WorkingIn { get; set; }
        public string Location { get; set; }
        public bool InSeason { get; set; }
        public long? MachineCode { get; set; }
        public bool Archived { get; set; }

        //FK
        //public int? CheckLogResultId { get; set; }
        public Guid MachineTypeId { get; set; }
        public string ApplicationUserId { get; set; }
        public MachineType MachineType { get; set; }
        public int MachineStatusId { get; set; }
        public Guid MachineCategoryId { get; set; }
        //public CheckLogResult CheckLogResult { get; set; }
        public string OperatorId { get; set; }
        public int ResultId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public MachineStatus MachineStatus { get; set; }
        public MachineCategory MachineCategory { get; set; }
        public ApplicationUser Operator { get; set; }
        public CheckResult CheckResult { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Issue> Issues { get; set; }
        public ICollection<MachineOperatorMapping> MachineOperatorMappings { get; set; }
        public ICollection<CheckListMachineMapping> CheckListMachineMappings { get; set; }
    }
}
