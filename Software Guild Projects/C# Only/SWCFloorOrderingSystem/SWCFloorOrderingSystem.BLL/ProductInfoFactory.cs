using SWCFloorOrderingSystem.Data;
using SWCFloorOrderingSystem.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.BLL
{
    public static class ProductInfoFactory
    {
        public static IProductInfo Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            switch (mode)
            {
                case "Test":
                    return new ProductRepoTest();
                case "Prod":
                    return new ProductRepoFile();
                default:
                    throw new Exception("Mode value in app settings isn't valid.");
            }
        }
    }
}
