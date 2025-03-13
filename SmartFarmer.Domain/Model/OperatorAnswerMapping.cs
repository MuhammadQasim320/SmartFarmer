using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class OperatorAnswerMapping
    {
        public Guid OperatorAnswerMappingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid AnswerId { get; set; }
        public string OperatorId { get; set; }
        //public Guid QuestionId { get; set; }
        public Answer Answer { get; set; }
        public ApplicationUser Operator { get; set; }
    }
}
