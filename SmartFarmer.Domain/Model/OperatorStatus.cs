using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class OperatorStatus
    {
        public int OperatorStatusId { get; set; }
        public string Status { get; set; }

        //FK
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }
}
