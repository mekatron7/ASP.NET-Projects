using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibraryWebAPI.Models
{
    public class DVDCatalogEntities : DbContext
    {
        public DVDCatalogEntities() : base("DVDCatalogEF")
        {
        }

        public DbSet<DVDView> DVDs { get; set; }
    }
}
