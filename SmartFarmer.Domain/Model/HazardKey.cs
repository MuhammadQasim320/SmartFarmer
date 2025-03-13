using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class HazardKey
    {
        public Guid HazardKeyId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public DateTime CreatedDate { get; set; }
        public int HazardTypeId { get; set; }
        public string CreatedBy { get; set; }
        public HazardType HazardType { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<HazardKeyFieldMapping> HazardKeyFieldMappings { get;}

    }
}
