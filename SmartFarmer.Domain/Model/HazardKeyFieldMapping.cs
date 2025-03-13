using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class HazardKeyFieldMapping
    {
        public Guid HazardKeyFieldMappingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Location { get; set; }
        public Guid HazardKeyId { get; set; }
        public Guid FieldId { get; set; }
        public HazardKey HazardKey { get; set; }
        public Field Field { get; set; }
    }
}
