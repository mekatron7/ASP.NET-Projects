using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChromebookManager.Models.Admin
{
    public class InventoryLogVM
    {
        public List<InventoryTransfer> InvTransfers { get; set; }
        public List<InvEditHistory> InvEditHistory { get; set; }
        public int InvEditId { get; set; }
        public string Notes { get; set; }
    }
}