using ChromebookManager.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChromebookManager.Models.Home
{
    public class RepairRequestVM
    {
        public int SchoolId { get; set; }
        public List<SelectListItem> Schools { get; set; }
        public string StudentUsername { get; set; }
        public bool Unassigned { get; set; }
        public int Grade { get; set; }
        public string Barcode { get; set; }
        public string SerialNumber { get; set; }
        public int IssueType { get; set; }
        public string IssueDescription { get; set; }
        public List<IssueType> IssueTypes { get; set; }
        public Alert Alert { get; set; }
        public bool FromClientProfile { get; set; }
    }
}