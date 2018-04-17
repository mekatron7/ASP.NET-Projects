using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerball
{
    interface IPickRepository
    {
        Pick Create(Pick p);
        Pick FindByID(int id);
        IEnumerable<Pick> FindBestMatches(Pick draw);
    }
}
