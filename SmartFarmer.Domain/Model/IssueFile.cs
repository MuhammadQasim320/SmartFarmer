using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class IssueFile
    {
        public Guid IssueFileId { get; set; }
        public string FileURL { get; set; }
        public string FileUniqueName { get; set; }
        //Fk
        public Guid IssueId { get; set; }
        public Issue Issue { get; set; }
    }
}
