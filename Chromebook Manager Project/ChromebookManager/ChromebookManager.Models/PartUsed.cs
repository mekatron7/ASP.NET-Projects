using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class PartUsed
    {
        public int PartUsedId { get; set; }
        public int RepairId { get; set; }
        public int ModelPartId { get; set; }
        public int SchoolId { get; set; }
        public string PartName { get; set; }
        public int MPCostId { get; set; }
        public decimal Cost { get; set; }
        public string SchoolName { get; set; }
        public bool Recycled { get; set; }
    }
}
