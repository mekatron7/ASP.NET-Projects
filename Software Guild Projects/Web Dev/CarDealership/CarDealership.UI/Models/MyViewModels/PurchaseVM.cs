using CarDealership.Data;
using CarDealership.UI.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealership.UI.Models.MyViewModels
{
    [PhoneOrEmail(ErrorMessage = "A phone number or email must be provided.")]
    [PurchasePrice]
    public class PurchaseVM
    {
        public Car Vehicle { get; set; }

        [Required]
        public string Name { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Street1 { get; set; }

        public string Street2 { get; set; }

        [Required]
        public string City { get; set; }

        public List<SelectListItem> States { get; set; }

        [Required]
        [StringLength(5)]
        public string Zip { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        public List<SelectListItem> PurchaseTypes { get; set; }

        [Required]
        public string SelectedState { get; set; }

        [Required]
        public string SelectedPurchaseType { get; set; }

        public void SetListItems()
        {
            States = new List<SelectListItem>
            {
                new SelectListItem { Text = "MN", Value = "MN" },
                new SelectListItem { Text = "WI", Value = "WI" },
                new SelectListItem { Text = "OH", Value = "OH" },
                new SelectListItem { Text = "IL", Value = "IL" },
                new SelectListItem { Text = "IN", Value = "IN" },
                new SelectListItem { Text = "MI", Value = "MI" },
                new SelectListItem { Text = "PA", Value = "PA" },
                new SelectListItem { Text = "MD", Value = "MD" },
                new SelectListItem { Text = "NY", Value = "NY" },
                new SelectListItem { Text = "NJ", Value = "NJ" },
                new SelectListItem { Text = "FL", Value = "FL" },
                new SelectListItem { Text = "NC", Value = "NC" },
                new SelectListItem { Text = "GA", Value = "GA" },
                new SelectListItem { Text = "TX", Value = "TX" },
                new SelectListItem { Text = "CA", Value = "CA" },
                new SelectListItem { Text = "WA", Value = "WA" },
            };

            PurchaseTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Bank Finance", Value = "Bank Finance" },
                new SelectListItem { Text = "Cash", Value = "Cash" },
                new SelectListItem { Text = "Dealer Finance", Value = "Dealer Finance" }
            };
        }
    }
}