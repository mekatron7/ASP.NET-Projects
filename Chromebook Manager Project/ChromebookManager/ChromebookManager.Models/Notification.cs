using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Username { get; set; }
        public DateTime NotifDate { get; set; }
        public string NotifType { get; set; }
        public string NotifMessage { get; set; }
        public bool Seen { get; set; }
        public string FromUsername { get; set; }
        public int? SchoolId { get; set; }
        public int? ModelPartId { get; set; }
        public int? Qty { get; set; }
        public bool Fulfillable { get; set; }
    }
}
