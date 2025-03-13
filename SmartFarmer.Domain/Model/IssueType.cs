using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class IssueType
    {
        public int IssueTypeId { get; set; }
        public string Type { get; set; }
        public ICollection<Issue> Issues { get; set; }
    }
}
