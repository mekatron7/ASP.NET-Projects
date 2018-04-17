using CarDealership.Data;
using CarDealership.UI.Models.MyViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealership.UI.Models.Attributes
{
    public class ModelExistsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is AddModelVM)
            {
                AddModelVM model = (AddModelVM)value;

                ICarRepo repo = CarRepoFactory.CreateRepo();
                List<string> models = repo.GetAllModels().Select(m => m.ModelName.ToLower()).ToList();
                List<string> editions = repo.GetAllModels().Where(m => m.ModelName == model.ModelName).Select(m => m.ModelEdition.ToLower()).ToList();

                if (models.Contains(model.ModelName.ToLower()) && editions.Contains(model.ModelEdition.ToLower()))
                {
                    ErrorMessage = $"The model '{model.ModelName} {model.ModelEdition}' already exists.";
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}