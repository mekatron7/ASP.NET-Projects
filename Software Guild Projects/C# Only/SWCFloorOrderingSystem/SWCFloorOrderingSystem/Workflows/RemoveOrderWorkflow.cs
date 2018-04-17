using SWCFloorOrderingSystem.BLL;
using SWCFloorOrderingSystem.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Workflows
{
    public class RemoveOrderWorkflow
    {
        public void Execute(RepositoryManager manager)
        {
            //Console.Clear();
            //Console.WriteLine("This section is currently under construction.\nFollow the bright orange cones to navigate the fuck up on outta here.\n*New Yawk accent* Cmonnnn we're working here!!");
            //Console.WriteLine();
            //Console.WriteLine("Press any key to continue.");
            //Console.ReadKey();

            Console.Clear();

            DateTime date;
            int orderNumber;

            while (true)
            {
                Console.WriteLine("Delete Order\n-----------------------------------------");
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
                Console.WriteLine("Delete Order\n-----------------------------------------");
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
                string result = "";

                while (result != "Y" && result != "N")
                {
                    Console.Clear();
                    Console.WriteLine("Delete Order\n-----------------------------------------");
                    ConsoleIO.DisplayOrder(lookupResponse.Order);
                    Console.WriteLine();
                    result = ConsoleIO.AreYouSure();
                }

                Console.WriteLine();

                if(result == "Y")
                {
                    DeleteOrderResponse response = manager.DeleteOrder(lookupResponse.Order);
                    Console.WriteLine(response.Message);
                }
                else
                {
                    Console.WriteLine("Your order will not be deleted from the system.");
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
