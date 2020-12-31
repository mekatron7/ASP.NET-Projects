using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class CostHistory
    {
        public int MPCostId { get; set; }
        public int ModelCostId { get; set; }
        public int ModelPartId { get; set; }
        public int ModelId { get; set; }
        public decimal Cost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModelPartName { get; set; }
        public string ModelName { get; set; }
    }
}
