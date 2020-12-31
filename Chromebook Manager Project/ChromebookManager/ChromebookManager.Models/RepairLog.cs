using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class RepairLog
    {
        public int RepairId { get; set; }
        public DateTime RepairTimestamp { get; set; }
        public string Username { get; set; }
        public int Grade { get; set; }
        public string Barcode { get; set; }
        public string SerialNumber { get; set; }
        public int ClientDeviceId { get; set; }
        public int IssueId { get; set; }
        public string IssueName { get; set; }
        public string IssueDescription { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string RepairNotes { get; set; }
        public DateTime? RepairReturnedDate { get; set; }
        public string EmailAddress { get; set; }
        public string Notes { get; set; }
        public DateTime? WarrantyRepairSentDate { get; set; }
        public string AddedBy { get; set; }
        public List<Part> PartsUsed { get; set; }
    }
}
