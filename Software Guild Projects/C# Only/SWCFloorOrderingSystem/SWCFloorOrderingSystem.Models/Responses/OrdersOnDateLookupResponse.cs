using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Models.Responses
{
    public class OrdersOnDateLookupResponse : Response
    {
        public List<Order> Orders { get; set; }

    }
}
