using SWCFloorOrderingSystem.Models;
using SWCFloorOrderingSystem.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Data
{
    public class ProductRepoTest : IProductInfo
    {
        public List<ProductInfo> GetProducts()
        {
            List<ProductInfo> products = new List<ProductInfo>();

            ProductInfo carpet = new ProductInfo
            {
                ProductType = "Carpet",
                CostPerSqFt = 2.25M,
                LaborCostPerSqFt = 2.10M
            };

            ProductInfo laminate = new ProductInfo
            {
                ProductType = "Laminate",
                CostPerSqFt = 1.75M,
                LaborCostPerSqFt = 2.10M
            };

            ProductInfo tile = new ProductInfo
            {
                ProductType = "Tile",
                CostPerSqFt = 3.50M,
                LaborCostPerSqFt = 4.15M
            };

            ProductInfo wood = new ProductInfo
            {
                ProductType = "Wood",
                CostPerSqFt = 5.15M,
                LaborCostPerSqFt = 4.75M
            };

            products.Add(carpet);
            products.Add(laminate);
            products.Add(tile);
            products.Add(wood);

            return products;
        }
    }
}
