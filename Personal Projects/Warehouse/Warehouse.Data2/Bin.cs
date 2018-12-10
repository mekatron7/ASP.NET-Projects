using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Data2
{
    public class Bin
    {
        public int BinId { get; set; }
        [Required]
        public string BinName { get; set; }
        [Required]
        public int Capacity { get; set; }
        public int AvailableSpace { get; set; }
    }
}
