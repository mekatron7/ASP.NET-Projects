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
        public int Qty { get; set; }
        public int OldBinId { get; set; }
        public int OldQty { get; set; }
        public List<SelectListItem> Products { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Bins { get; set; } = new List<SelectListItem>();

        public void SetListItems(string mode)
        {
            var wr = new WarehouseRepo();
            if (mode == "create")
            {
                var list = new List<SelectListItem>();
                foreach (var prod in wr.GetProducts())
                {
                    list.Add(new SelectListItem { Text = prod.SKU, Value = prod.ProductId.ToString() });
                }
                Products = list;
            }

            var list2 = new List<SelectListItem>();
            foreach (var bin in wr.GetBins())
            {
                list2.Add(new SelectListItem { Text = bin.BinName, Value = bin.BinId.ToString() });
            }
            Bins = list2;
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