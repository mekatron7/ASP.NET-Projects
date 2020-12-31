using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChromebookManager.Models.Admin
{
    public class PurchaseOrderVM
    {
        public List<PurchaseOrder> PurchaseOrders { get; set; }
        public List<PurchaseOrderLI> LineItems { get; set; }
        public long PONumber { get; set; }
        public int POLineItemId { get; set; }
        public int Qty { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? DateReceived { get; set; }
        public Alert Alert { get; set; }
        public List<SelectListItem> PartsSelectList { get; set; }
        public int SelectedPart { get; set; }
        public int AddToPOQty { get; set; }
        public DateTime DateOrdered { get; set; }
    }
}