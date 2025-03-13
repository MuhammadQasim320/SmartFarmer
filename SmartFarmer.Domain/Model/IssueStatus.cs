using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class IssueStatus
    {
        public int IssueStatusId { get; set; }
        public string Status { get; set; }
        public ICollection<Issue> Issues { get; set; }
    }
}
