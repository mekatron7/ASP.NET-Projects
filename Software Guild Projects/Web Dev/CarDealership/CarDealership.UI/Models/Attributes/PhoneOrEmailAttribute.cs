using CarDealership.UI.Models.MyViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealership.UI.Models.Attributes
{
    public class PhoneOrEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is PurchaseVM)
            {
                PurchaseVM model = (PurchaseVM)value;

                if(string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Phone))
                {
                    return false;
                }

                return true;
            }
            else if(value is ContactVM)
            {
                ContactVM model = (ContactVM)value;

                if (string.IsNullOrEmpty(model.ContactEmail) && string.IsNullOrEmpty(model.ContactPhone))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}