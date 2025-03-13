using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class MachineCategory
    {
        public Guid MachineCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        //FK
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Machine> Machines { get; set; }
    }
}
