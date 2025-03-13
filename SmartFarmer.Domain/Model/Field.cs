using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class Field
    {
        public Guid FieldId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Center { get; set; }
        public string Boundary { get; set; }

        //FK

        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public Guid FarmId { get; set; }
        public Farm Farm { get; set; }

        public ICollection<HazardKeyFieldMapping> HazardKeyFieldMappings { get; }

    }
}
