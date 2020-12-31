using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class Model
    {
        public int ModelId { get; set; }
        public string ModelNumber { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
        public decimal Cost { get; set; }
    }
}
