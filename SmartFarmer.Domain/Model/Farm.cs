using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class Farm
    {
        public Guid FarmId { get; set; }
        public string FarmName { get; set; }
        public DateTime CreatedDate { get; set; }


        //Fk
        public string MasterAdminId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string CreatedBy { get; set; }
        public ApplicationUser CreatedByUser { get; set; }
        public ICollection<Field> Fields { get; set; }
    }
}
