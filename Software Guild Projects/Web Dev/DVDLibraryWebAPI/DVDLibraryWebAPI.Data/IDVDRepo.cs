using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibraryWebAPI.Models
{
    public interface IDVDRepo
    {
        List<DVDView> GetAll();
        void Create(DVDView dvd);
        void Update(DVDView updatedDVD);
        bool Delete(int id);
        List<DVDView> GetSearch(string category, string term);
    }
}
