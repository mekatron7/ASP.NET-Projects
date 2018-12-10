using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data2;

namespace Warehouse.Models
{
    public class AddToOrderVM
    {
        public Product Prod { get; set; }
        public int ProdId { get; set; }
        [Required]
        public int OrderNum { get; set; }
        [Required]
        public int BinId { get; set; }
        [Required]
        public int Qty { get; set; }
        public List<SelectListItem> Orders { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Bins { get; set; } = new List<SelectListItem>();


        private WarehouseRepo wr = new WarehouseRepo();

        public void SetAddToOrderLists()
        {
            foreach(var order in wr.GetOrders())
            {
                Orders.Add(new SelectListItem { Text = $"Order #{order.OrderNumber}: {order.CustomerName}", Value = order.OrderNumber.ToString() });
            }

            var binInv = wr.GetInventory(0, Prod.ProductId, 0);
            foreach(var bin in binInv)
            {
                var binInfo = bin.GetBinInfo();
                Bins.Add(new SelectListItem { Text = $"{binInfo.BinName} | Qty: {bin.Qty}", Value = binInfo.BinId.ToString() });
            }
        }
    }
}