using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Data2
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public DateTime DateOrdered { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
    }
}
