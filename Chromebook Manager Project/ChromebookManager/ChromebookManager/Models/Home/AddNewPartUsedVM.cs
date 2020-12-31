using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChromebookManager.Models.Home
{
    public class AddNewPartUsedVM
    {
        public int Index { get; set; }
        public List<SelectListItem> InventoryList { get; set; }
        public List<string> AddedPartsUsed { get; set; }
        public bool PartChosen { get; set; }
    }
}