namespace SmartFarmer.Domain.Model
{
    public class CheckList
    {
        public Guid CheckListId { get; set; }
        public string Name { get; set; }
        public string Frequency { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastCheckDate { get; set; }
        public DateTime? NextDueDate { get; set; }

        //FK
        public int FrequencyTypeId { get; set; }
        public int CheckTypeId { get; set; }
        public Guid MachineTypeId { get; set; }
        public string CreatedBy { get; set; }
        public string OperatorId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ApplicationUser Operator { get; set; }
        public FrequencyType FrequencyType { get; set; }
        public CheckType CheckType { get; set; }
        public MachineType MachineType { get; set; }
        public ICollection<CheckListItem> CheckListItems { get; set; }
        public ICollection<CheckListMachineMapping> CheckListMachineMappings { get; set; }
    }
}
