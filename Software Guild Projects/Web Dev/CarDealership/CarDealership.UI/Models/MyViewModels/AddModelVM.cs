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
    [ModelExists]
    public class AddModelVM
    {
        public List<SelectListItem> Makes { get; set; } = new List<SelectListItem>();

        [Required]
        public string SelectedMake { get; set; }

        [Required]
        public string ModelName { get; set; }

        [Required]
        [Edition]
        public string ModelEdition { get; set; }

        public void SetMakeItems(List<Make> makes)
        {
            foreach(var make in makes)
            {
                Makes.Add(new SelectListItem()
                {
                    Value = make.MakeId.ToString(),
                    Text = make.MakeName
                });
            }
        }
    }
}