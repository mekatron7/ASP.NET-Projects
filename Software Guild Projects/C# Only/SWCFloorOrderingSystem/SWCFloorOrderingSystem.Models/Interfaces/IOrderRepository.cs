using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Models.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> LoadAllOrders(DateTime date);
        Order LoadOrder(DateTime date, int orderNumber);
        void SaveNewOrder(Order order);
        void SaveEditedOrder(Order order);
        void DeleteOrder(Order order);
    }
}
