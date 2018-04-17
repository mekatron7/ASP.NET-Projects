using SWCFloorOrderingSystem.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Models.Responses
{
    public class AddOrderResponse : Response
    {
        public Order NewOrder { get; set; }
        public IOrderRepository OrderRepo { get; set; }
    }
}
