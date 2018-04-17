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
    public class ProductRepoFile : IProductInfo
    {
        public List<ProductInfo> GetProducts()
        {
            string filePath = @"C:\Data\SystemIO\FlooringOrders\Products.txt";

            string[] productLines = File.ReadAllLines(filePath);
            List<ProductInfo> productList = new List<ProductInfo>();

            for(int i = 1; i < productLines.Length; i++)
            {
                string[] productDetails = productLines[i].Split(',');
                ProductInfo p = new ProductInfo
                {
                    ProductType = productDetails[0],
                    CostPerSqFt = decimal.Parse(productDetails[1]),
                    LaborCostPerSqFt = decimal.Parse(productDetails[2])
                };

                productList.Add(p);
            }

            return productList;
        }
    }
}
