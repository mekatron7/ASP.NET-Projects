using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Models
{
    public class TaxInfo
    {
        public string StateAbbr { get; set; }
        public string StateName { get; set; }
        public decimal TaxRate { get; set; }
    }
}
