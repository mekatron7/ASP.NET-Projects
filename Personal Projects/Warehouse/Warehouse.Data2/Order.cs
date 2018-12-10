using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Data2
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public DateTime DateOrdered { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerAddress { get; set; }

        private WarehouseRepo wr = new WarehouseRepo();

        public List<OrderLine> GetOrderLines()
        {
            return wr.GetOrderLines(OrderNumber);
        }
    }
}
