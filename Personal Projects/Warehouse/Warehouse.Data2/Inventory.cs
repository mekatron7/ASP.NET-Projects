using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Data2
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int BinId { get; set; }
        public int Qty { get; set; }

        private WarehouseRepo wr = new WarehouseRepo();

        public Product GetProductInfo()
        {
            return wr.GetProduct(ProductId);
        }

        public Bin GetBinInfo()
        {
            return wr.GetBin(BinId, "");
        }
    }
}
