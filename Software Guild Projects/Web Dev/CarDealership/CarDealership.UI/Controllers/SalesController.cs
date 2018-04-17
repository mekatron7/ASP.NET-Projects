using CarDealership.Data;
using CarDealership.UI.Models.MyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealership.UI.Controllers
{
    [Authorize(Roles ="sales, admin")]
    public class SalesController : Controller
    {
        // GET: Sales
        [HttpGet]
        public ActionResult Index()
        {
            ICarRepo repo = CarRepoFactory.CreateRepo();
            var model = repo.GetAllCars().Where(c => c.Purchased == "N").ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Purchase(string VIN)
        {
            ICarRepo repo = CarRepoFactory.CreateRepo();
            var car = repo.Get(VIN);
            var model = new PurchaseVM();
            model.Vehicle = car;
            model.SetListItems();
            return View(model);
        }

        [HttpPost]
        public ActionResult Purchase(PurchaseVM purchase)
        {
            ICarRepo repo = CarRepoFactory.CreateRepo();

            if (ModelState.IsValid)
            {
                var newPurchase = new Purchase()
                {
                    VIN_ = purchase.Vehicle.VIN_,
                    PurchaseName = purchase.Name,
                    PurchaseEmail = purchase.Email,
                    PurchasePhone = purchase.Phone,
                    PurchasePrice = purchase.PurchasePrice,
                    PurchaseType = purchase.SelectedPurchaseType,
                    PState = purchase.SelectedState,
                    City = purchase.City,
                    Street1 = purchase.Street1,
                    Street2 = purchase.Street2,
                    ZipCode = int.Parse(purchase.Zip),
                    PurchaseDate = DateTime.Now,
                    PurchaseUser = User.Identity.Name
                };

                var car = repo.Get(purchase.Vehicle.VIN_);
                car.Purchased = "Y";
                car.Featured = "N";
                repo.EditCar(car);

                repo.AddPurchase(newPurchase);
                return RedirectToAction("Index");
            }
            purchase.Vehicle = repo.Get(purchase.Vehicle.VIN_);
            purchase.SetListItems();
            return View(purchase);
        }
    }
}