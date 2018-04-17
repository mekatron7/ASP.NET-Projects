using CarDealership.UI.Models.MyViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealership.UI.Models.Attributes
{
    public class MileageAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is AddEditVehicleVM)
            {
                AddEditVehicleVM model = (AddEditVehicleVM)value;

                if(model.SelectedType == "New" && (model.Mileage < 0 || model.Mileage > 1000))
                {
                    ErrorMessage = "The mileage must be between 0 and 1000 miles for new vehicles.";
                    return false;
                }

                if(model.SelectedType == "Used" && model.Mileage < 1000)
                {
                    ErrorMessage = "The mileage must be 1000+ miles for used vehicles.";
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}