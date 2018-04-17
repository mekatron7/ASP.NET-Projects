using CarDealership.UI.Models.MyViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealership.UI.Models.Attributes
{
    public class MSRPSalePriceAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is AddEditVehicleVM)
            {
                AddEditVehicleVM model = (AddEditVehicleVM)value;

                if(model.MSRP < 0 || model.SalePrice < 0)
                {
                    ErrorMessage = "The MSRP and Sale Price cannot be negative.";
                    return false;
                }

                if(model.SalePrice > model.MSRP)
                {
                    ErrorMessage = "The sale price cannot exceed the MSRP";
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}