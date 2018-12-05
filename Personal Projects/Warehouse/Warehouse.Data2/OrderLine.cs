using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Data2
{
    public class OrderLine
    {
        public int OrderLineId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }

        private WarehouseRepo wr = new WarehouseRepo();

        public Order GetOrderInfo()
        {
            return wr.GetOrder(OrderId);
        }

        public Product GetProductInfo()
        {
            return wr.GetProduct(ProductId);
        }
    }
}
