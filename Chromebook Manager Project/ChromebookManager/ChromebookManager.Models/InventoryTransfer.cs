using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class InventoryTransfer
    {
        public int InvTransferId { get; set; }
        public string PartName { get; set; }
        public string FromSchoolName { get; set; }
        public string ToSchoolName { get; set; }
        public int FromSchool { get; set; }
        public int ToSchool { get; set; }
        public int ModelPartId { get; set; }
        public DateTime TransferDate { get; set; }
        public int Qty { get; set; }
        public string Username { get; set; }
        public bool Recycled { get; set; }
    }
}
