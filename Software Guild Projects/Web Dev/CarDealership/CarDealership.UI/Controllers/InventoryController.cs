using CarDealership.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealership.UI.Controllers
{
    [AllowAnonymous]
    public class InventoryController : Controller
    {
        ICarRepo repo = CarRepoFactory.CreateRepo();

        // GET: New Inventory
        public ActionResult New()
        {      
            return View(repo.GetAllNewCars().Take(20).ToList());
        }

        public ActionResult Used()
        {
            return View(repo.GetAllUsedCars().Take(20).ToList());
        }

        [HttpGet]
        public ActionResult Details(string VIN)
        {
            return View(repo.Get(VIN));
        }
    }
}