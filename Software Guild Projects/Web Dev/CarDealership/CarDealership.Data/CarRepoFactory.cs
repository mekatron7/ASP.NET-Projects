using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CarDealership.Data
{
    public class CarRepoFactory
    {
        public static ICarRepo CreateRepo()
        {
            string mode = ConfigurationManager.AppSettings["Mode"];

            switch (mode)
            {
                case "EF":
                    return new EFRepo();
                case "Memory":
                    return new InMemoryRepo();
                default:
                    throw new Exception("Mode value in app config isn't supported.");
            }
        }
    }
}