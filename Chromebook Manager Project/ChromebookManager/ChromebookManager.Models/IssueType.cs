using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class IssueType
    {
        public int IssueId { get; set; }
        public string IssueName { get; set; }
        public string IssueDescription { get; set; }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
    }
}
