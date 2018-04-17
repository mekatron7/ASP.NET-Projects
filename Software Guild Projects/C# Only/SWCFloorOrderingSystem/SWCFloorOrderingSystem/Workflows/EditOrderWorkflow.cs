using SWCFloorOrderingSystem.BLL;
using SWCFloorOrderingSystem.Models;
using SWCFloorOrderingSystem.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Workflows
{
    public class EditOrderWorkflow
    {
        public void Execute(RepositoryManager manager)
        {
            Console.Clear();

            DateTime date;
            int orderNumber;
            string name;
            string state;
            string productType;
            decimal area;

            while (true)
            {
                Console.WriteLine("Edit Order\n-----------------------------------------");
                Console.WriteLine("Enter in a valid date in MM/DD/YYYY format.");
                Console.Write("Date: ");
                if (DateTime.TryParse(Console.ReadLine(), out date))
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("You must enter a valid date.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Edit Order\n-----------------------------------------");
                Console.WriteLine("Enter in the order number you want to look up and hit Enter.");
                Console.Write("Order #: ");
                if (int.TryParse(Console.ReadLine(), out orderNumber))
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("You must enter a numerical value.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            SingleOrderLookupResponse lookupResponse = manager.LookupOrder(date, orderNumber);

            if (lookupResponse.Success)
            {
                Order orderCopy = new Order
                {
                    OrderNumber = lookupResponse.Order.OrderNumber,
                    OrderDate = lookupResponse.Order.OrderDate,
                    CustomerName = lookupResponse.Order.CustomerName,
                    State = lookupResponse.Order.State,
                    TaxRate = lookupResponse.Order.TaxRate,
                    ProductType = lookupResponse.Order.ProductType,
                    Area = lookupResponse.Order.Area,
                    CostPerSquareFoot = lookupResponse.Order.CostPerSquareFoot,
                    LaborCostPerSquareFoot = lookupResponse.Order.LaborCostPerSquareFoot
                };

                Console.WriteLine();
                Console.WriteLine($"Current order info for {lookupResponse.Order.CustomerName}:");
                ConsoleIO.DisplayOrder(lookupResponse.Order);
                Console.WriteLine("Press any key to begin editing your order.");
                Console.ReadKey();

                //Edit Customer Name
                Console.Clear();
                Console.WriteLine("Edit Order\n-----------------------------------------");
                Console.WriteLine("Enter your full name or company name.");
                Console.Write("Name: ");
                name = Console.ReadLine();

                if (name != "")
                {
                    orderCopy.CustomerName = name;
                }

                //Edit State
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Edit Order\n-----------------------------------------");
                    Console.WriteLine("Enter your state of residence by its two letter state code (MN).");
                    Console.WriteLine("Type 'I' to view available states and their tax rates.");
                    Console.Write("State: ");
                    state = Console.ReadLine();
                    if (state == "I" || state == "i")
                    {
                        ConsoleIO.DisplayStates();
                    }
                    else
                    {
                        Console.Clear();
                        break;
                    }
                }

                if (state != "")
                {
                    orderCopy.State = state;
                }

                //Edit Product Type
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Edit Order\n-----------------------------------------");
                    Console.WriteLine("Enter the product type you wish to purchase.");
                    Console.WriteLine("Type 'I' to view product information.");
                    Console.Write("Product Type: ");
                    productType = Console.ReadLine();
                    if (productType == "I" || productType == "i")
                    {
                        ConsoleIO.DisplayProducts();
                    }
                    else
                    {
                        Console.Clear();
                        break;
                    }
                }

                if (productType != "")
                {
                    orderCopy.ProductType = productType;
                }

                //Edit Area
                while (true)
                {
                    Console.WriteLine("Edit Order\n-----------------------------------------");
                    Console.WriteLine("Enter a value for the area of flooring being covered.\nArea is measured in sq ft.");
                    Console.Write("Area: ");
                    string input = Console.ReadLine();
                    if(input == "")
                    {
                        area = 0;
                        break;
                    }
                    else if (decimal.TryParse(input, out area))
                    {
                        orderCopy.Area = area;
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("You must enter a valid numerical value.");
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }

                EditOrderResponse response = manager.EditOrder(orderCopy);

                if (response.Success)
                {
                    string saveOrder = ConsoleIO.ReviewOrderEdit(lookupResponse, response);

                    if (saveOrder == "Y")
                    {
                        response.OrderRepo.SaveEditedOrder(response.OrderEdit);

                        Console.Clear();
                        Console.WriteLine("Your changes have been saved!");

                        ConsoleIO.DisplayOrder(response.OrderEdit);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Any changes you made to your order have been discarded.");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("An error occured:");
                    Console.WriteLine(response.Message);
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(lookupResponse.Message);
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
