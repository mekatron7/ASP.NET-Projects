using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Models.Responses
{
    public class SingleOrderLookupResponse : Response
    {
        public Order Order { get; set; }
    }
}
