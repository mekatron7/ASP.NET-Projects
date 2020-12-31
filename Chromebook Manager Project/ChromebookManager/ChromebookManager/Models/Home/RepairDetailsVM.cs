using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChromebookManager.Models.Home
{
    public class RepairDetailsVM
    {
        public RepairLog RepairLog { get; set; }
        public Client Client { get; set; }
        public List<PartUsed> PartsUsed { get; set; }
        public List<SelectListItem> PartsSelectList { get; set; }
        public List<string> AddedPartsUsed { get; set; }
        public string Model { get; set; }
    }
}