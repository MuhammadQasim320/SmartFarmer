using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class TrainingRecord
    {
        public Guid TrainingRecordId { get; set; }
        public string Name { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Expires { get; set; }
        public int? Validity { get; set; }
        public bool Certification { get; set; }
        public string Qualification { get; set; }
        public bool Archived { get; set; }
        public string Description { get; set; }
        public int TrainingTypeId { get; set; }
        public string CreatedBy { get; set; }
        public TrainingType TrainingType { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<TrainingRecordOperatorMapping> TrainingRecordOperatorMappings { get; set; }
    }
}
