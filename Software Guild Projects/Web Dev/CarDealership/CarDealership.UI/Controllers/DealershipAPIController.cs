using CarDealership.Data;
using CarDealership.UI.Models.MyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarDealership.UI.Controllers
{
    public class DealershipAPIController : ApiController
    {
        [Route("cars/getmodels")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetModels(string make)
        {
            ICarRepo repo = CarRepoFactory.CreateRepo();
            try
            {
                Make m = repo.GetAllMakes().Single(i => i.MakeName == make);
                var models = m.Models.Select(a => new
                {
                    a.ModelName
                }).Distinct().ToList();

                if(models.Count() == 0)
                {
                    return BadRequest();
                }

                return Ok(models);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }   
        }

        [Route("cars/geteditions")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetEditions(string make, string model)
        {
            ICarRepo repo = CarRepoFactory.CreateRepo();
            try
            {
                Make m = repo.GetAllMakes().Single(i => i.MakeName == make);
                IEnumerable<Model> mod = repo.GetAllModels(m).Where(i => i.ModelName == model);
                var editions = mod.Select(a => new
                {
                    a.ModelEdition
                }).Distinct().ToList();

                return Ok(editions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("cars/search")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Search(string quickSearch, int minPrice, int maxPrice, int minYear, int maxYear, string type)
        {
            try
            {
                ICarRepo repo = CarRepoFactory.CreateRepo();
                List<Car> allCars = repo.GetAllCars();
                List<Car> results;

                if (string.IsNullOrEmpty(quickSearch))
                {
                    if (minPrice == 0 && maxPrice == 0 && minYear == 0 && maxYear == 0)
                    {
                        results = allCars;
                    }
                    else
                    {
                        if (maxPrice == 0)
                        {
                            maxPrice = int.MaxValue;
                        }
                        if (maxYear == 0)
                        {
                            maxYear = DateTime.Today.Year + 1;
                        }

                        results = allCars.Where(c => c.SalePrice >= minPrice && c.SalePrice <= maxPrice && c.CarYear >= minYear && c.CarYear <= maxYear).ToList();
                    }
                }
                else
                {
                    if (maxPrice == 0)
                    {
                        maxPrice = int.MaxValue;
                    }
                    if (maxYear == 0)
                    {
                        maxYear = DateTime.Today.Year + 1;
                    }

                    if (int.TryParse(quickSearch, out int year))
                    {
                        results = allCars.Where(c => c.CarYear == year && c.SalePrice > minPrice && c.SalePrice < maxPrice).ToList(); 
                    }
                    else
                    {
                        results = allCars.Where(c => c.Model.Make.MakeName.ToLower().Contains(quickSearch.ToLower()) && c.SalePrice >= minPrice && c.SalePrice <= maxPrice && c.CarYear >= minYear && c.CarYear <= maxYear
                            || c.Model.ModelName.ToLower().Contains(quickSearch.ToLower()) && c.SalePrice >= minPrice && c.SalePrice <= maxPrice && c.CarYear >= minYear && c.CarYear <= maxYear).ToList();
                    }
                }

                if (type != "admin")
                {
                    switch (type)
                    {
                        case "new":
                            results = results.Where(c => c.CarType == "New").ToList();
                            break;
                        case "used":
                            results = results.Where(c => c.CarType == "Used").ToList();
                            break;
                        case "sales":
                            results = results.Where(c => c.Purchased == "N").ToList();
                            break;
                        default:
                            results = results.OrderByDescending(m => m.CarYear).ToList();
                            break;
                    }
                }

                var results2 = results.Select(car => new
                {
                    VIN = car.VIN_,
                    car.BodyStyle.BSName,
                    car.CarDescription,
                    car.CarPicture,
                    car.CarType,
                    car.CarYear,
                    car.ExteriorColor.ExtColorName,
                    car.ExteriorColor.ExtColorType,
                    car.Featured,
                    car.InteriorColor.IntColorName,
                    car.InteriorColor.IntColorType,
                    car.Mileage,
                    car.Model.Make.MakeName,
                    car.Model.ModelName,
                    car.Model.ModelEdition,
                    MSRP = string.Format("{0:c}", car.MSRP),
                    car.Purchased,
                    SalePrice = string.Format("{0:c}", car.SalePrice),
                    car.Transmission.TransmissionType
                }).Take(20).ToList();

                return Ok(results2);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
