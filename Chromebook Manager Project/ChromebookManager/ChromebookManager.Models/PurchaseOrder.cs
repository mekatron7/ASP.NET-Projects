using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class PurchaseOrder
    {
        public long PONumber { get; set; }
        public string Username { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TotalQty { get; set; }
        public decimal TotalCost { get; set; }
        public string Notes { get; set; }
        public int MPCostId { get; set; }
        public int NumOfLIs { get; set; }
        public int NumOfPendingLIs { get; set; }
    }
}
