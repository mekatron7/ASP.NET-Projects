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
    [Mileage]
    [MSRPSalePrice]
    public class AddEditVehicleVM
    {
        public List<SelectListItem> Makes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Types { get; set; }

        [Required]
        [AddCarYear]
        public int? Year { get; set; }

        public List<SelectListItem> Colors { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ColorTypes { get; set; }

        [Required]
        public int? Mileage { get; set; }

        [Required]
        public decimal? MSRP { get; set; }

        [Required(ErrorMessage = "The Sale Price field is required.")]
        public decimal? SalePrice { get; set; }

        public List<SelectListItem> Models { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Editions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> BodyStyles { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Transmissions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> IntColors { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> IntColorTypes { get; set; }

        public string VIN { get; set; }

        [Required]
        [StringLength(17)]
        [Display(Name = "VIN")]
        public string FormVIN { get; set; }

        [Required]
        public string Description { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public string CarPicture { get; set; }

        public string Purchased { get; set; }

        [Required(ErrorMessage = "The Make field is required.")]
        public string SelectedMake { get; set; }

        [Required(ErrorMessage = "The Type field is required.")]
        public string SelectedType { get; set; }

        [Required(ErrorMessage = "The Color field is required.")]
        public string SelectedColor { get; set; }

        [Required(ErrorMessage = "The Model field is required.")]
        public string SelectedModel { get; set; }

        [Required(ErrorMessage = "The Body Style field is required.")]
        public string SelectedBodyStyle { get; set; }

        [Required(ErrorMessage = "The Transmission field is required.")]
        public string SelectedTransmision { get; set; }

        [Required(ErrorMessage = "The Interior Color field is required.")]
        public string SelectedIntColor { get; set; }

        [Required(ErrorMessage = "The Color Type field is required.")]
        public string SelectedColorType { get; set; }

        [Required(ErrorMessage = "The Interior Color Type field is required.")]
        public string SelectedIntColorType { get; set; }

        [Required(ErrorMessage = "The Edition field is required.")]
        public string SelectedEdition { get; set; }

        public bool Featured { get; set; }

        public void SetListItems()
        {
            ICarRepo repo = CarRepoFactory.CreateRepo();

            //Set Makes Dropdown List
            foreach (var make in repo.GetAllMakes())
            {
                Makes.Add(new SelectListItem()
                {
                    Value = make.MakeName,
                    Text = make.MakeName
                });
            }

            //Set Types Dropdown List
            Types = new List<SelectListItem>
            {
                new SelectListItem { Text = "New", Value = "New" },
                new SelectListItem { Text = "Used", Value = "Used" }
            };

            //Set Colors Dropdown List
            foreach (var color in repo.GetExtColors())
            {
                Colors.Add(new SelectListItem()
                {
                    Value = color,
                    Text = color
                });
            }

            //Set ColorTypes Dropdown List
            ColorTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Matte", Value = "Matte" },
                new SelectListItem { Text = "Gloss", Value = "Gloss" }
            };

            //Set BodyStyles Dropdown List
            foreach (var bodyStyle in repo.GetBodyStyles())
            {
                BodyStyles.Add(new SelectListItem()
                {
                    Value = bodyStyle,
                    Text = bodyStyle
                });
            }

            //Set Transmissions Dropdown List
            foreach (var trans in repo.GetTransmissions())
            {
                Transmissions.Add(new SelectListItem()
                {
                    Value = trans,
                    Text = trans
                });
            }

            //Set IntColors Dropdown List
            foreach (var color in repo.GetIntColors())
            {
                IntColors.Add(new SelectListItem()
                {
                    Value = color,
                    Text = color
                });
            }

            //Set IntColorTypes Dropdown List
            IntColorTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Cloth", Value = "Cloth" },
                new SelectListItem { Text = "Leather", Value = "Leather" },
                new SelectListItem { Text = "Suede", Value = "Suede" },
                new SelectListItem { Text = "Gucci", Value = "Gucci" }
            };
        }

        public void ResetModelsAndEditions(string makeName, string modelName)
        {
            ICarRepo repo = CarRepoFactory.CreateRepo();

            Make make = repo.GetAllMakes().Single(m => m.MakeName == makeName);
            List<string> models = repo.GetAllModels(make).Select(m => m.ModelName).Distinct().ToList();
            List<Model> modelEditions = repo.GetAllModels().Where(m => m.ModelName == modelName).ToList();

            foreach(var model in models)
            {
                Models.Add(new SelectListItem()
                {
                    Value = model,
                    Text = model
                });
            }

            foreach(var edition in modelEditions)
            {
                Editions.Add(new SelectListItem()
                {
                    Value = edition.ModelEdition,
                    Text = edition.ModelEdition
                });
            }
        }
    }
}