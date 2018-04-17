using SWCFloorOrderingSystem.Models;
using SWCFloorOrderingSystem.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.Data
{
    public class TaxRepoTest : ITaxInfo
    {
        public List<TaxInfo> GetTaxInfo()
        {
            List<TaxInfo> states = new List<TaxInfo>();

            TaxInfo minnesota = new TaxInfo
            {
                StateName = "Minnesota",
                StateAbbr = "MN",
                TaxRate = 6.875M,
            };

            TaxInfo michigan = new TaxInfo
            {
                StateName = "Michigan",
                StateAbbr = "MI",
                TaxRate = 5.75M,
            };

            TaxInfo ohio = new TaxInfo
            {
                StateName = "Ohio",
                StateAbbr = "OH",
                TaxRate = 6.25M,
            };

            TaxInfo pennsylvania = new TaxInfo
            {
                StateName = "Pennsylvania",
                StateAbbr = "PA",
                TaxRate = 6.75M,
            };

            TaxInfo indiana = new TaxInfo
            {
                StateName = "Indiana",
                StateAbbr = "IN",
                TaxRate = 6.00M,
            };

            states.Add(minnesota);
            states.Add(michigan);
            states.Add(ohio);
            states.Add(pennsylvania);
            states.Add(indiana);

            return states;
        }
    }
}
