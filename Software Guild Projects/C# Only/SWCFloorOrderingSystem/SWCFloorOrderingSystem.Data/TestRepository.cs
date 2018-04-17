using SWCFloorOrderingSystem.Models;
using SWCFloorOrderingSystem.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Data
{
    public class TestRepository : IOrderRepository
    {
        public List<Order> Orders { get; set; } = new List<Order>();

        public TestRepository()
        {
            Order order1 = new Order
            {
                CustomerName = "Jack Andackster",
                OrderNumber = 0,
                OrderDate = DateTime.Parse("4-12-18"),
                State = "MN",
                TaxRate = 6.875M,
                ProductType = "Wood",
                Area = 350,
                CostPerSquareFoot = 5.15M,
                LaborCostPerSquareFoot = 4.75M
            };

            Order order2 = new Order
            {
                CustomerName = "Tiga Woods",
                OrderNumber = 1,
                OrderDate = DateTime.Parse("4-12-18"),
                State = "PA",
                TaxRate = 6.75M,
                ProductType = "Wood",
                Area = 420,
                CostPerSquareFoot = 5.15M,
                LaborCostPerSquareFoot = 4.75M
            };

            Order order3 = new Order
            {
                CustomerName = "Jennifer Aniston",
                OrderNumber = 0,
                OrderDate = DateTime.Parse("6-16-18"),
                State = "OH",
                TaxRate = 6.25M,
                ProductType = "Tile",
                Area = 700,
                CostPerSquareFoot = 3.50M,
                LaborCostPerSquareFoot = 4.15M
            };

            Order order4 = new Order
            {
                CustomerName = "Trill Nye",
                OrderNumber = 2,
                OrderDate = DateTime.Parse("4-12-18"),
                State = "IN",
                TaxRate = 6.00M,
                ProductType = "Carpet",
                Area = 650,
                CostPerSquareFoot = 2.25M,
                LaborCostPerSquareFoot = 2.10M
            };

            Order order5 = new Order
            {
                CustomerName = "Mack Spaine",
                OrderNumber = 3,
                OrderDate = DateTime.Parse("4-12-18"),
                State = "MN",
                TaxRate = 6.875M,
                ProductType = "Laminate",
                Area = 440,
                CostPerSquareFoot = 1.75M,
                LaborCostPerSquareFoot = 2.10M
            };

            Orders.Add(order1);
            Orders.Add(order2);
            Orders.Add(order3);
            Orders.Add(order4);
            Orders.Add(order5);
        }

        public List<Order> LoadAllOrders(DateTime date)
        {
            List<Order> ordersOnDate = Orders.Where(i => i.OrderDate == date).ToList();

            if(ordersOnDate.Count() == 0)
            {
                return null;
            }

            return ordersOnDate;
        }

        public Order LoadOrder(DateTime date, int orderNumber)
        {
            return Orders.SingleOrDefault(i => i.OrderDate == date && i.OrderNumber == orderNumber);
        }

        public void SaveEditedOrder(Order order)
        {
            Orders.Remove(Orders.Single(i => i.OrderNumber == order.OrderNumber && i.OrderDate == order.OrderDate));
            Orders.Add(order);
            Orders = Orders.OrderBy(i => i.OrderDate).ThenBy(i => i.OrderNumber).ToList();
        }

        public void SaveNewOrder(Order order)
        {
            if(Orders.Any(i => i.OrderDate == order.OrderDate))
            {
                int orderNumber = Orders.Where(i => i.OrderDate == order.OrderDate).Max(i => i.OrderNumber) + 1;
                order.OrderNumber = orderNumber;
            }
            
            Orders.Add(order);
        }

        public void DeleteOrder(Order order)
        {
            Orders.Remove(Orders.Where(i => i.OrderDate == order.OrderDate).SingleOrDefault(i => i.OrderNumber == order.OrderNumber));
        }
    }
}
