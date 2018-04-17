using NUnit.Framework;
using SWCFloorOrderingSystem.BLL;
using SWCFloorOrderingSystem.Models;
using SWCFloorOrderingSystem.Models.Interfaces;
using SWCFloorOrderingSystem.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Tests
{
    [TestFixture]
    public class ProdRepoTests
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
        public void CanLoadFileData()
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
        public void ProdAddOrder(string date, string name, string state, string productType, decimal area, bool expectedResult)
        {
            RepositoryManager repo = RepositoryManagerFactory.Create();
            DateTime orderDate = DateTime.Parse(date);

            AddOrderResponse response = repo.AddOrder(orderDate, name, state, productType, area);
            Assert.AreEqual(expectedResult, response.Success);
        }

        [TestCase("5-06-18", 0, "Harry Pohttuh", "MI", "Laminate", 650, true)]
        [TestCase("4-12-18", 2, "Eddie Mamme", "OH", "Suede", 400, false)]
        [TestCase("4-12-18", 5, "Tiga Woods", "CA", "Wood", 2000, false)]
        [TestCase("4-12-18", 8, "Tackz Siding", "MN", "Wood", 1000, true)]
        public void ProdEditOrder(string date, int orderNumber, string name, string state, string PT, decimal area, bool expectedResult)
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

        [TestCase("5-22-18", 0, true)]
        [TestCase("4-12-18", 8, true)]
        public void ProdDeleteOrder(string date, int orderNumber, bool expectedResult)
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
