using NUnit.Framework;
using SWCFloorOrderingSystem.BLL;
using SWCFloorOrderingSystem.Models;
using SWCFloorOrderingSystem.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Tests
{
    class TestRepoTests
    {
        [SetUp]
        public void ResetTestData()
        {
            string dir = @"C:\Data\SystemIO\FlooringOrdersTests\";
            string workingDir = @"C:\Data\SystemIO\FlooringOrders\";

            string[] seedFilePaths = Directory.GetFiles(dir);

            foreach (string seedFilePath in seedFilePaths)
            {
                //using FileInfo to get the file name without the directory
                FileInfo seedFileInfo = new FileInfo(seedFilePath);

                //generate the path to the equivalent file in the working folder
                string destFilePath = Path.Combine(workingDir, seedFileInfo.Name);

                //copy the file (the true argument here tells it to overwrite if present)
                File.Copy(seedFilePath, destFilePath, true);
            }
        }

        [Test]
        public void CanLoadTestData()
        {
            RepositoryManager manager = RepositoryManagerFactory.Create();

            OrdersOnDateLookupResponse response = manager.LookupOrdersOnDate(DateTime.Parse("4-12-18"));

            Assert.IsNotNull(response.Orders);
            Assert.IsTrue(response.Success);
        }

        [TestCase("4-12-18", "Trill Nye", "OH", "Laminate", 650, true)]
        [TestCase("6-01-18", "Fox McCloud", "NJ", "Carpet", 400, false)]
        [TestCase("4-12-17", "Cloud Strife", "OH", "Tile", 300, false)]
        [TestCase("5-09-18", "Ilona Wexler", "MN", "Wood", 700, true)]
        public void TestAddOrder(string date, string name, string state, string productType, decimal area, bool expectedResult)
        {
            RepositoryManager repo = RepositoryManagerFactory.Create();
            DateTime orderDate = DateTime.Parse(date);

            AddOrderResponse response = repo.AddOrder(orderDate, name, state, productType, area);
            Assert.AreEqual(expectedResult, response.Success);
        }

        [TestCase("6-16-18", 0, "Rachel Fromfriends", "OH", "Laminate", 777, true)]
        [TestCase("4-12-18", 2, "Trill Nye", "OH", "Gucci", 420, false)]
        [TestCase("4-12-18", 0, "Jaq Andackster", "Minnesota", "Wood", 800, false)]
        [TestCase("4-12-18", 1, "Tiga Woods", "MN", "Wood", 1000, true)]
        public void TestEditOrder(string date, int orderNumber, string name, string state, string PT, decimal area, bool expectedResult)
        {
            RepositoryManager repo = RepositoryManagerFactory.Create();
            DateTime orderDate = DateTime.Parse(date);

            Order o = repo.LookupOrder(orderDate, orderNumber).Order;

            Order orderCopy = new Order
            {
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                CustomerName = name,
                State = state,
                TaxRate = o.TaxRate,
                ProductType = PT,
                Area = area,
                CostPerSquareFoot = o.CostPerSquareFoot,
                LaborCostPerSquareFoot = o.LaborCostPerSquareFoot
            };

            EditOrderResponse response = repo.EditOrder(orderCopy);
            Assert.AreEqual(expectedResult, response.Success);
        }

        [TestCase("6-16-18", 0, true)]
        [TestCase("4-12-18", 3, true)]
        public void TestDeleteOrder(string date, int orderNumber, bool expectedResult)
        {
            RepositoryManager repo = RepositoryManagerFactory.Create();
            DateTime orderDate = DateTime.Parse(date);

            Order o = repo.LookupOrder(orderDate, orderNumber).Order;

            DeleteOrderResponse response = repo.DeleteOrder(o);
            Assert.IsNotNull(o);
            Assert.AreEqual(expectedResult, response.Success);
        }
    }
}
