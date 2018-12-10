using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Warehouse.Data2;

namespace Warehouse.Models
{
    public class BinContents
    {
        public List<Inventory> Contents { get; set; }
        public string BinName { get; set; }
    }
}