using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data2;

namespace Warehouse.Models
{
    public class InventoryVM
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int BinId { get; set; }
        public int OldBinId { get; set; }
        public int Qty { get; set; }
        public int OldQty { get; set; }
        public List<SelectListItem> Products { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Bins { get; set; } = new List<SelectListItem>();

        public void SetListItems()
        {
            var wr = new WarehouseRepo();
            foreach (var prod in wr.GetProducts())
            {
                Products.Add(new SelectListItem { Text = $"{prod.SKU} | {prod.ProductDescription}", Value = prod.ProductId.ToString() });
            }

            foreach (var bin in wr.GetBins())
            {
                Bins.Add(new SelectListItem { Text = bin.BinName, Value = bin.BinId.ToString() });
            }
        }

        public void SetTransferListItems()
        {
            var wr = new WarehouseRepo();

            foreach(var bin in wr.GetBins().Where(b => b.BinId != OldBinId))
            {
                Bins.Add(new SelectListItem { Text = bin.BinName, Value = bin.BinId.ToString() });
            }
        }

        public Product GetProductInfo()
        {
            var wr = new WarehouseRepo();
            return wr.GetProduct(ProductId);
        }

        public Bin GetBinInfo()
        {
            var wr = new WarehouseRepo();
            return wr.GetBin(BinId, "");
        }

        public Bin GetOldBinInfo()
        {
            var wr = new WarehouseRepo();
            return wr.GetBin(OldBinId, "");
        }
    }
}