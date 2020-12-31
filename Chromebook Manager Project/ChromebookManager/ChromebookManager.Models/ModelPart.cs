using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class ModelPart
    {
        public int ModelPartId { get; set; }
        public string PartName { get; set; }
        public string ModelNumber { get; set; }
        public string BrandName { get; set; }
        public decimal Cost { get; set; }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
        public int ModelId { get; set; }
        public int PartId { get; set; }
        public int MPCostId { get; set; }
    }
}
