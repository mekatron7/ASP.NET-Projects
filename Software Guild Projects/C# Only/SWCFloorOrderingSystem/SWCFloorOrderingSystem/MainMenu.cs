using SWCFloorOrderingSystem.BLL;
using SWCFloorOrderingSystem.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem
{
    public static class MainMenu
    {
        public static void Start()
        {
            //Instantiate the Manager and make it equal to the Manager Factory calling its Create method
            RepositoryManager manager = RepositoryManagerFactory.Create();
            //Chooses the correct repository to use

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the SWC Floordering System");
                Console.WriteLine("You'll be floored by our quality and our prices!");
                Console.WriteLine();
                Console.WriteLine("Select an option below by typing its corresponding number\nand hit Enter.");
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine("1.) Display Orders");
                Console.WriteLine("2.) Add an Order");
                Console.WriteLine("3.) Edit an Order");
                Console.WriteLine("4.) Delete an Order");
                Console.WriteLine("5.) Exit");
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.Write("Selection: ");
                string userInput = Console.ReadLine().ToUpper();

                switch (userInput)
                {
                    case "1":
                        DisplayOrdersWorkflow displayOrders = new DisplayOrdersWorkflow();
                        displayOrders.Execute(manager);
                        break;
                    case "2":
                        AddOrderWorkflow addOrders = new AddOrderWorkflow();
                        addOrders.Execute(manager);
                        break;
                    case "3":
                        EditOrderWorkflow editOrder = new EditOrderWorkflow();
                        editOrder.Execute(manager);
                        break;
                    case "4":
                        RemoveOrderWorkflow removeOrder = new RemoveOrderWorkflow();
                        removeOrder.Execute(manager);
                        break;
                    case "5":
                        return;

                }
            }
        }
    }
}
