using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string BrandName { get; set; }
        public string ModelNumber { get; set; }
        public string PartName { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string SchoolName { get; set; }
        public int Qty { get; set; }
        public int RecycledQty { get; set; }
        public DateTime DateLastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public int ModelPartId { get; set; }
        public int SchoolId { get; set; }
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public int PartId { get; set; }

        public Inventory() { }

        public Inventory(Inventory inv)
        {
            InventoryId = inv.InventoryId;
            BrandName = inv.BrandName;
            ModelNumber = inv.ModelNumber;
            PartName = inv.PartName;
            UnitCost = inv.UnitCost;
            TotalCost = inv.TotalCost;
            SchoolName = inv.SchoolName;
            Qty = inv.Qty;
            RecycledQty = inv.RecycledQty;
            DateLastModified = inv.DateLastModified;
            LastModifiedBy = inv.LastModifiedBy;
            ModelPartId = inv.ModelPartId;
            SchoolId = inv.SchoolId;
            BrandId = inv.BrandId;
            ModelId = inv.ModelId;
            PartId = inv.PartId;
        }

        public Inventory Copy()
        {
            return new Inventory(this);
        }
    }
}
