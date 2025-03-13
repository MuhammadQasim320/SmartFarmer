using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class CheckListMachineMapping
    {
        public Guid CheckListMachineMappingId { get; set; }
        public DateTime CreatedDate { get; set; }
        //FK
        public Guid CheckListId { get; set; }
        public virtual CheckList CheckList { get; set; }
        public Guid MachineId { get; set; }
        public Machine Machine { get; set; }
        public string OperatorId { get; set; }
        public ApplicationUser Operator { get; set; }
        public int ResultId { get; set; }
        public CheckResult CheckResult { get; set; }
        public ICollection<CheckListItemAnswerMapping> CheckListItemAnswerMappings { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
