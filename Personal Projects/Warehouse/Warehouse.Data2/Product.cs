using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Data2
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(7, ErrorMessage = "The SKU can't exceed 7 characters.")]
        public string SKU { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public int Size { get; set; }

        private WarehouseRepo wr = new WarehouseRepo();

        public bool HasInventory()
        {
            return wr.GetInventory(0, ProductId, 0).Count > 0;
        }

        public int GetInvQuantity()
        {
            return wr.GetInventory(0, ProductId, 0).Sum(p => p.Qty);
        }
    }
}
