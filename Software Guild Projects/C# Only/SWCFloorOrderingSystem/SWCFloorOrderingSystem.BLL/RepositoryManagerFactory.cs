using SWCFloorOrderingSystem.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.BLL
{
    public static class RepositoryManagerFactory
    {
        public static RepositoryManager Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            switch (mode)
            {
                case "Test":
                    return new RepositoryManager(new TestRepository());
                case "Prod":
                    return new RepositoryManager(new ProdRepository());
                default:
                    throw new Exception("Mode value in app config isn't valid.");
            }
        }
    }
}
