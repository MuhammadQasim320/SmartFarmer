using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class MachineStatus
    {
        public int MachineStatusId { get; set; }
        public string Status { get; set; }
        public ICollection<Machine> Machines { get; set; }

    }
}
