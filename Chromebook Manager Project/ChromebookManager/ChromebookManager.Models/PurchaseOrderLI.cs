using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class PurchaseOrderLI
    {
        public int POLineItemId { get; set; }
        public long PONumber { get; set; }
        public int ModelPartId { get; set; }
        public string BrandName { get; set; }
        public string ModelNumber { get; set; }
        public string PartName { get; set; }
        public int Qty { get; set; }
        public int MPCostId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? DateReceived { get; set; }
    }
}
