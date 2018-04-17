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
    public class TaxRepoFile : ITaxInfo
    {
        public List<TaxInfo> GetTaxInfo()
        {
            string filePath = @"C:\Data\SystemIO\FlooringOrders\Taxes.txt";

            string[] taxLines = File.ReadAllLines(filePath);
            List<TaxInfo> taxList = new List<TaxInfo>();

            for(int i = 1; i < taxLines.Length; i++)
            {
                string[] taxDetails = taxLines[i].Split(',');
                TaxInfo t = new TaxInfo
                {
                    StateAbbr = taxDetails[0],
                    StateName = taxDetails[1],
                    TaxRate = decimal.Parse(taxDetails[2])
                };

                taxList.Add(t);
            }

            return taxList;
        }
    }
}
