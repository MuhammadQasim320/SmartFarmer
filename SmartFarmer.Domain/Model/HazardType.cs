using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class HazardType
    {
        public int HazardTypeId { get; set; }
        public string Type { get; set; }
        public ICollection<HazardKey> HazardKeys { get; set; }
    }
}
