using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealership.UI.Models.Attributes
{
    public class EditionAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is string)
            {
                string editionField = (string)value;

                if (editionField.Contains(","))
                {
                    string[] editions = editionField.Split(',');

                    foreach(var edition in editions)
                    {
                        if(edition.Length > 10)
                        {
                            ErrorMessage = $"The edition {edition} exceeds 10 charaters.";
                            return false;
                        }

                        if(edition.Length == 0)
                        {
                            ErrorMessage = $"You comma delimited your editions incorrectly. Try again.";
                            return false;
                        }
                    }

                    return true;
                }
                else if (editionField.Length > 10)
                {
                    ErrorMessage = "The edition name must be 10 characters or less.";
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}