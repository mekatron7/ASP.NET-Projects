using SWCFloorOrderingSystem.BLL;
using SWCFloorOrderingSystem.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Workflows
{
    public class AddOrderWorkflow
    {
        public void Execute(RepositoryManager manager)
        {
            Console.Clear();

            DateTime date;
            string name;
            string state;
            string productType;
            decimal area;

            while (true)
            {
                Console.WriteLine("Add New Order\n-----------------------------------------");
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
                Console.WriteLine("Add New Order\n-----------------------------------------");
                Console.WriteLine("Enter your full name or company name.");
                Console.Write("Name: ");
                name = Console.ReadLine();

                if(name != "")
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("You cannot leave the name field blank.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Add New Order\n-----------------------------------------");
                Console.WriteLine("Enter your state of residence by its two letter state code (MN).");
                Console.WriteLine("Type 'I' to view available states and their tax rates.");
                Console.Write("State: ");
                state = Console.ReadLine();
                if(state == "I" || state == "i")
                {
                    ConsoleIO.DisplayStates();
                }
                else
                {
                    Console.Clear();
                    break;
                }
            }
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Add New Order\n-----------------------------------------");
                Console.WriteLine("Enter the product type you wish to purchase.");
                Console.WriteLine("Type 'I' to view product information.");
                Console.Write("Product Type: ");
                productType = Console.ReadLine();
                if(productType == "I" || productType == "i")
                {
                    ConsoleIO.DisplayProducts();
                }
                else
                {
                    Console.Clear();
                    break;
                }
            }
            

            while (true)
            {
                Console.WriteLine("Add New Order\n-----------------------------------------");
                Console.WriteLine("Enter a value for the area of flooring being covered.\nArea is measured in sq ft.");
                Console.Write("Area: ");
                if(decimal.TryParse(Console.ReadLine(), out area))
                {
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

            AddOrderResponse response = manager.AddOrder(date, name, state, productType, area);

            if (response.Success)
            {
                string saveOrder = ConsoleIO.ReviewOrder(response);

                if(saveOrder == "Y")
                {
                    response.OrderRepo.SaveNewOrder(response.NewOrder);

                    Console.Clear();
                    Console.WriteLine("Your order had been processed!");

                    ConsoleIO.DisplayOrder(response.NewOrder);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Ok fine don't order anything then...your loss.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("An error occured:");
                Console.WriteLine(response.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
