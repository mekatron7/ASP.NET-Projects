using CarDealership.UI.Models.MyViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealership.UI.Models.Attributes
{
    public class AddCarYearAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is int?)
            {
                int year = (int)value;

                if(year >= 2000 && year <= DateTime.Now.Year + 1)
                {
                    return true;
                }

                ErrorMessage = $"The car year must be on or between the years 2000 and {DateTime.Now.Year + 1}.";
                return false;
            }

            return false;
        }
    }
}