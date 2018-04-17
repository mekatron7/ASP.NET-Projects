using CarDealership.UI.Models.MyViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealership.UI.Models.Attributes
{
    public class PurchasePriceAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is PurchaseVM)
            {
                PurchaseVM model = (PurchaseVM)value;

                if(model.PurchasePrice < model.Vehicle.SalePrice * .95m)
                {
                    ErrorMessage = "The purchase price cannot be lower than 95% of the sales price.";
                    return false;
                }

                if(model.PurchasePrice > model.Vehicle.MSRP)
                {
                    ErrorMessage = "The purchase price cannot exceed the MSRP.";
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}