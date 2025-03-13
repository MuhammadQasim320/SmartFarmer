using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class CheckListItemAnswerMapping
    {
        public Guid CheckListItemAnswerMappingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Answer { get; set; }
        //Fk
        public Guid CheckListItemId { get; set; }
        public CheckListItem CheckListItem { get; set; }
        public Guid CheckListMachineMappingId { get; set; }
        public CheckListMachineMapping CheckListMachineMapping { get; set; }
        
    }
}
