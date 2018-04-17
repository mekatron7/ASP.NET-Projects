using SWCFloorOrderingSystem.Models;
using SWCFloorOrderingSystem.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Data
{
    public class ProdRepository : IOrderRepository
    {
        private static string _fileDir = @"C:\Data\SystemIO\FlooringOrders\";

        public List<Order> LoadAllOrders(DateTime date)
        {
            string filePath = $"{_fileDir}Orders_{date.ToString("MMddyyyy")}.txt";

            List<Order> orders = new List<Order>();

            if (!File.Exists(filePath))
            {
                return null;
            }
            else
            {
                string[] ordersThisDay = File.ReadAllLines(filePath);

                for(int i = 1; i < ordersThisDay.Length; i++)
                {
                    string[] orderDetails = ordersThisDay[i].Split(',');

                    Order o = new Order
                    {
                        OrderDate = date,
                        OrderNumber = int.Parse(orderDetails[0]),
                        CustomerName = orderDetails[1],
                        State = orderDetails[2],
                        TaxRate = decimal.Parse(orderDetails[3]),
                        ProductType = orderDetails[4],
                        Area = decimal.Parse(orderDetails[5]),
                        CostPerSquareFoot = decimal.Parse(orderDetails[6]),
                        LaborCostPerSquareFoot = decimal.Parse(orderDetails[7])
                    };

                    orders.Add(o);
                }
                return orders;
            }
        }

        public Order LoadOrder(DateTime date, int orderNumber)
        {
            var orderList = LoadAllOrders(date);
            if(orderList == null)
            {
                return null;
            }

            return orderList.SingleOrDefault(i => i.OrderNumber == orderNumber);
        }

        public void SaveEditedOrder(Order order)
        {
            string filePath = $"{_fileDir}Orders_{order.OrderDate.ToString("MMddyyyy")}.txt";

            var updatedOrderList = LoadAllOrders(order.OrderDate);

            //Order updatedOrder = updatedOrderList.Single(i => i.OrderNumber == order.OrderNumber);
            //updatedOrder = order;
            updatedOrderList.Remove(updatedOrderList.Single(i => i.OrderNumber == order.OrderNumber));
            updatedOrderList.Add(order);
            updatedOrderList = updatedOrderList.OrderBy(i => i.OrderNumber).ToList();

            string[] updateFile = new string[updatedOrderList.Count() + 1];
            int index = 1;

            updateFile[0] = "Order Number,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot," +
                "MaterialCost,LaborCost,Tax,Total";

            foreach(Order o in updatedOrderList)
            {
                updateFile[index] = $"{o.OrderNumber},{o.CustomerName},{o.State},{o.TaxRate},{o.ProductType}," +
                    $"{o.Area},{o.CostPerSquareFoot},{o.LaborCostPerSquareFoot},{o.MaterialCost},{o.LaborCost}," +
                    $"{o.Tax},{o.Total}";
                index++;
            }

            File.WriteAllLines(filePath, updateFile);
        }

        public void SaveNewOrder(Order order)
        {
            string filePath = $"{_fileDir}Orders_{order.OrderDate.ToString("MMddyyyy")}.txt";

            int orderNumber = 0;

            if (File.Exists(filePath))
            {
                var allOrders = LoadAllOrders(order.OrderDate);

                orderNumber = allOrders.Max(i => i.OrderNumber) + 1;
                order.OrderNumber = orderNumber;

                string newEntry = $"{order.OrderNumber},{order.CustomerName},{order.State},{order.TaxRate},{order.ProductType}," +
                    $"{order.Area},{order.CostPerSquareFoot},{order.LaborCostPerSquareFoot},{order.MaterialCost},{order.LaborCost}," +
                    $"{order.Tax},{order.Total}{Environment.NewLine}";

                File.AppendAllText(filePath, newEntry);
            }
            else
            {
                string header = "Order Number,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot," +
                "MaterialCost,LaborCost,Tax,Total" + Environment.NewLine;

                string newEntry = $"{order.OrderNumber},{order.CustomerName},{order.State},{order.TaxRate},{order.ProductType}," +
                    $"{order.Area},{order.CostPerSquareFoot},{order.LaborCostPerSquareFoot},{order.MaterialCost},{order.LaborCost}," +
                    $"{order.Tax},{order.Total}{Environment.NewLine}";

                File.WriteAllText(filePath, header);
                File.AppendAllText(filePath, newEntry);
            }
        }

        public void DeleteOrder(Order order)
        {
            var orderRemovedList = LoadAllOrders(order.OrderDate);
            //orderRemovedList.Remove(order);
            orderRemovedList.Remove(orderRemovedList.Single(i => i.OrderNumber == order.OrderNumber));

            string filePath = $"{_fileDir}Orders_{order.OrderDate.ToString("MMddyyyy")}.txt";

            if(orderRemovedList.Count() == 0)
            {
                File.Delete(filePath);
            }
            else
            {
                string[] updateFile = new string[orderRemovedList.Count() + 1];
                int index = 1;

                updateFile[0] = "Order Number,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot," +
                    "MaterialCost,LaborCost,Tax,Total";

                foreach (Order o in orderRemovedList)
                {
                    updateFile[index] = $"{o.OrderNumber},{o.CustomerName},{o.State},{o.TaxRate},{o.ProductType}," +
                        $"{o.Area},{o.CostPerSquareFoot},{o.LaborCostPerSquareFoot},{o.MaterialCost},{o.LaborCost}," +
                        $"{o.Tax},{o.Total}";
                    index++;
                }

                File.WriteAllLines(filePath, updateFile);
            }
        }
    }
}
