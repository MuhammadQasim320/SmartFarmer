using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class TrainingOperatorMapping
    {
        public Guid TrainingOperatorMappingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid TrainingId { get; set; }
        public string OperatorId { get; set; }
        public Training Training { get; set; }
        public ApplicationUser Operator { get; set; }
    }
}
