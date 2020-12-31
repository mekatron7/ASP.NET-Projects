using ChromebookManager.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChromebookManager.Models.Home
{
    public class InventoryVM
    {
        public List<Inventory> InventoryList { get; set; }
        public List<SelectListItem> SchoolSelectList { get; set; }
        public List<SelectListItem> ModelPartSelectList { get; set; }
        public int InventoryId { get; set; }
        public int ModelPartId { get; set; }
        public int SchoolId { get; set; }
        public int Qty { get; set; }
        public string FromSchool { get; set; }
        public string ToSchool { get; set; }
        public string PartName { get; set; }
        public List<Alert> Alerts { get; set; } = new List<Alert>();
        public string Notes { get; set; }
        public bool Recycled { get; set; }
        public int RecycledQty { get; set; }
    }
}