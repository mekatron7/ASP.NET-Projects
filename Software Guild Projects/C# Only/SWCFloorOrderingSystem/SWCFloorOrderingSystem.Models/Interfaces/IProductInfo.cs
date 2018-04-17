using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Models.Interfaces
{
    public interface IProductInfo
    {
        List<ProductInfo> GetProducts();
    }
}
