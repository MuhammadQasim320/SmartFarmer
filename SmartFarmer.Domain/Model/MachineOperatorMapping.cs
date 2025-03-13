using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class MachineOperatorMapping
    {
        public Guid MachineOperatorMappingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid MachineId { get; set; }
        public string OperatorId { get; set; }
        public bool IsActive { get; set; }
        public Machine Machine { get; set; }
        public ApplicationUser Operator { get; set; }
    }
}
