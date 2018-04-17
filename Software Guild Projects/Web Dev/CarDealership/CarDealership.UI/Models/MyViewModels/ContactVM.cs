using CarDealership.UI.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealership.UI.Models.MyViewModels
{
    [PhoneOrEmail(ErrorMessage = "An email or phone number must be provided.")]
    public class ContactVM
    {
        [Required(ErrorMessage = "The name field is required.")]
        public string ContactName { get; set; }

        [EmailAddress]
        public string ContactEmail { get; set; }

        [Phone]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "The message field is required.")]
        public string ContactMessage { get; set; }

        [StringLength(17)]
        public string VIN { get; set; }
    }
}