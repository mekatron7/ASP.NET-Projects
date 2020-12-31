using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class InvEditHistory
    {
        public int InvEditId { get; set; }
        public string PartName { get; set; }
        public string School { get; set; }
        public int OldQty { get; set; }
        public int NewQty { get; set; }
        public string Notes { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public bool Recycled { get; set; }
    }
}
