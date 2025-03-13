using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class CheckResult
    {
        public int ResultId { get; set; }
        public string Result { get; set; }

        //FK
        public ICollection<Machine> Machines { get; set; }
        public ICollection<CheckListMachineMapping> CheckListMachineMappings { get; set; }
    }
}
