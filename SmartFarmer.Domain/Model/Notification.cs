using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public string ToId { get; set; }
        public ApplicationUser ToUser { get; set; }
    }
}
