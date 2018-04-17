using SWCFloorOrderingSystem.BLL;
using SWCFloorOrderingSystem.Models;
using SWCFloorOrderingSystem.Models.Interfaces;
using SWCFloorOrderingSystem.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem
{
    public static class ConsoleIO
    {
        public static void NoOrdersOnRecord(DateTime date)
        {
            Console.Clear();
            Console.WriteLine($"There are no orders on record for {date.ToShortDateString()}");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        public static void DisplayOrders(List<Order> orders)
        {
            Console.WriteLine();
            foreach(Order o in orders)
            {
                DisplayOrder(o);
            }
        }

        public static void DisplayOrder(Order order)
        {
            Console.WriteLine("*****************************************************************************");
            Console.WriteLine($"Order #: {order.OrderNumber} || {order.OrderDate.ToShortDateString()}");
            Console.WriteLine($"Customer Name: {order.CustomerName}");
            Console.WriteLine($"State: {order.State}");
            Console.WriteLine($"Product: {order.ProductType}");
            Console.WriteLine($"Area: {order.Area} sq ft");
            Console.WriteLine($"Materials: {order.MaterialCost:c}");
            Console.WriteLine($"Labor: {order.LaborCost:c}");
            Console.WriteLine($"Tax: {order.Tax:c}");
            Console.WriteLine($"Total: {order.Total:c}");
            Console.WriteLine("*****************************************************************************");
            Console.WriteLine();
        }

        public static string ReviewOrder(AddOrderResponse response)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Order summary for {response.NewOrder.CustomerName}:");
                Console.WriteLine();
                Console.WriteLine("*****************************************************************************");
                Console.WriteLine($"{response.NewOrder.OrderDate.ToShortDateString()}");
                Console.WriteLine($"Customer Name: {response.NewOrder.CustomerName}");
                Console.WriteLine($"State: {response.NewOrder.State}");
                Console.WriteLine($"Product: {response.NewOrder.ProductType}");
                Console.WriteLine($"Area: {response.NewOrder.Area} sq ft");
                Console.WriteLine($"Materials: {response.NewOrder.MaterialCost:c}");
                Console.WriteLine($"Labor: {response.NewOrder.LaborCost:c}");
                Console.WriteLine($"Tax: {response.NewOrder.Tax:c}");
                Console.WriteLine($"Total: {response.NewOrder.Total:c}");
                Console.WriteLine("*****************************************************************************");
                Console.WriteLine();

                Console.WriteLine("Do you want to proceed with your order?\nType 'Y' for yes or 'N' for no.");
                Console.Write("Place order?: ");
                string input = Console.ReadLine().ToUpper();

                if (input == "Y" || input == "N")
                {
                    return input;
                }
            }
        }

        public static string ReviewOrderEdit(SingleOrderLookupResponse ogResponse, EditOrderResponse response)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Original order summary for {ogResponse.Order.CustomerName}:");
                Console.WriteLine();
                Console.WriteLine("*****************************************************************************");
                Console.WriteLine($"{ogResponse.Order.OrderDate.ToShortDateString()}");
                Console.WriteLine($"Customer Name: {ogResponse.Order.CustomerName}");
                Console.WriteLine($"State: {ogResponse.Order.State}");
                Console.WriteLine($"Product: {ogResponse.Order.ProductType}");
                Console.WriteLine($"Area: {ogResponse.Order.Area} sq ft");
                Console.WriteLine($"Materials: {ogResponse.Order.MaterialCost:c}");
                Console.WriteLine($"Labor: {ogResponse.Order.LaborCost:c}");
                Console.WriteLine($"Tax: {ogResponse.Order.Tax:c}");
                Console.WriteLine($"Total: {ogResponse.Order.Total:c}");
                Console.WriteLine("*****************************************************************************");
                Console.WriteLine();

                Console.WriteLine($"Modified order summary for {response.OrderEdit.CustomerName}:");
                Console.WriteLine();
                Console.WriteLine("*****************************************************************************");
                Console.WriteLine($"{response.OrderEdit.OrderDate.ToShortDateString()}");
                Console.WriteLine($"Customer Name: {response.OrderEdit.CustomerName}");
                Console.WriteLine($"State: {response.OrderEdit.State}");
                Console.WriteLine($"Product: {response.OrderEdit.ProductType}");
                Console.WriteLine($"Area: {response.OrderEdit.Area} sq ft");
                Console.WriteLine($"Materials: {response.OrderEdit.MaterialCost:c}");
                Console.WriteLine($"Labor: {response.OrderEdit.LaborCost:c}");
                Console.WriteLine($"Tax: {response.OrderEdit.Tax:c}");
                Console.WriteLine($"Total: {response.OrderEdit.Total:c}");
                Console.WriteLine("*****************************************************************************");
                Console.WriteLine();

                Console.WriteLine("Do the changes made in your order look right to you?\nType 'Y' for yes or 'N' for no.");
                Console.Write("Finalize edit?: ");
                string input = Console.ReadLine().ToUpper();

                if (input == "Y" || input == "N")
                {
                    return input;
                }
            }
        }

        public static string AreYouSure()
        {
            Console.WriteLine("Type 'Y' to delete order. Type 'N' to go back to the main menu.");
            Console.Write("Are you sure you want to delete this order?: ");

            string input = Console.ReadLine().ToUpper();

            if (input == "Y")
            {
                Console.WriteLine();
                Console.WriteLine("Are you REALLY sure? Once the order is deleted, it's gone forever.");
                Console.WriteLine("Type 'Y' to delete order. Type 'N' to go back to the main menu.");
                Console.Write("Delete order?: ");
                input = Console.ReadLine().ToUpper();
            }

            return input;
        }

        public static void DisplayProducts()
        {
            Console.Clear();
            IProductInfo products = ProductInfoFactory.Create();
            List<ProductInfo> productInfo = products.GetProducts();

            Console.WriteLine("Product and pricing info for our flooring options:");
            Console.WriteLine();

            foreach(ProductInfo p in productInfo)
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine($"Type: {p.ProductType}");
                Console.WriteLine($"Cost Per Sq Ft: {p.CostPerSqFt:c}");
                Console.WriteLine($"Labor Cost Per Sq Ft: {p.LaborCostPerSqFt:c}");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine();
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        public static void DisplayStates()
        {
            Console.Clear();
            ITaxInfo getTaxes = TaxInfoFactory.Create();
            List<TaxInfo> taxInfo = getTaxes.GetTaxInfo();

            Console.WriteLine("SWC is authorized to conduct business in the following states:");
            Console.WriteLine();

            foreach(TaxInfo t in taxInfo)
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine($"State: {t.StateName}");
                Console.WriteLine($"State Abbreviation: {t.StateAbbr:c}");
                Console.WriteLine($"Tax Rate: {t.TaxRate}%");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine();
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
