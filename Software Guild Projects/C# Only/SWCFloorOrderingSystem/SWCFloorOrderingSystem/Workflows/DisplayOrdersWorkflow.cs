using SWCFloorOrderingSystem.BLL;
using SWCFloorOrderingSystem.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Workflows
{
    public class DisplayOrdersWorkflow
    {
        public void Execute(RepositoryManager manager)
        {
            DateTime orderDate;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Display Orders\n-----------------------------------------");
                Console.Write("Enter a date in the form of MM/DD/YYYY: ");
                string input = Console.ReadLine();
                if(DateTime.TryParse(input, out orderDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("That was not a valid date. Please enter a date in MM/DD/YYYY format.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
            }

            //Call Manager to pass the date to the Display Workflow and get the orders
            OrdersOnDateLookupResponse response = manager.LookupOrdersOnDate(orderDate);

            if (response.Success)
            {
                Console.Clear();
                Console.WriteLine($"Orders made on {orderDate.ToShortDateString()}:");
                ConsoleIO.DisplayOrders(response.Orders);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("An error occurred:");
                Console.WriteLine(response.Message);
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
