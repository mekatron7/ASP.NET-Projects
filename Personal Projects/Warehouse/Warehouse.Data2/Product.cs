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
        [StringLength(7, ErrorMessage = "The SKU can't exceed 7 characters.")]
        public string SKU { get; set; }
        public string ProductDescription { get; set; }
        public int Size { get; set; }
    }
}
