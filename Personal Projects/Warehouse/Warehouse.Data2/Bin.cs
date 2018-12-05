using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Data2
{
    public class Bin
    {
        public int BinId { get; set; }
        public string BinName { get; set; }
        public int Capacity { get; set; }
        public int AvailableSpace { get; set; }
    }
}
