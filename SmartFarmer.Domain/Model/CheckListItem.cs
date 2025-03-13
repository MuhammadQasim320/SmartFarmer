namespace SmartFarmer.Domain.Model
{
    public class CheckListItem
    {
        public Guid CheckListItemId { get; set; }
        public string Name { get; set; }
        public string Instruction { get; set; }
        public string Order { get; set; }
        public int Priority { get; set; }

        //FK
        public Guid CheckListId { get; set; }
        public CheckList CheckList { get; set; }
        public ICollection<CheckListItemAnswerMapping> ChecListItemAnswerMappings { get; set; }
    }
}
