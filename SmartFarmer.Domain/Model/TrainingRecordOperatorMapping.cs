using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class TrainingRecordOperatorMapping
    {
        public Guid TrainingRecordOperatorMappingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid TrainingRecordId { get; set; }
        public string OperatorId { get; set; }
        public TrainingRecord TrainingRecord { get; set; }
        public ApplicationUser Operator { get; set; }
    }
}
