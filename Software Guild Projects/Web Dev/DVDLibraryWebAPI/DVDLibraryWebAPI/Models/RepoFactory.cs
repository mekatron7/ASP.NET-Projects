using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DVDLibraryWebAPI.Models
{
    public static class RepoFactory
    {
        public static IDVDRepo CreateRepo()
        {
            string mode = ConfigurationManager.AppSettings["Mode"];

            switch (mode)
            {
                case "Memory":
                    return new InMemoryRepo();
                case "ADO":
                    return new ADONETRepo();
                case "EF":
                    return new EFRepo();
                default:
                    throw new Exception("Mode value in app config isn't supported.");
            }
        }
    }
}