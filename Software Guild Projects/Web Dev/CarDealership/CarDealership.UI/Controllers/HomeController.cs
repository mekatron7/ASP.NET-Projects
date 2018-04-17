using CarDealership.Data;
using CarDealership.UI.Models.MyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealership.UI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ICarRepo repo = CarRepoFactory.CreateRepo();
            var model = new HomePageVM();
            model.FeaturedCars = repo.GetAllCars().Where(c => c.Featured == "Y").ToList();
            model.Specials = repo.GetAllSpecials();
            return View(model);
        }

        public ActionResult Specials()
        {
            ViewBag.Message = "The cars you really want with prices to make you want them even more.";

            var repository = new CarDealershipContext();
            var specials = repository.Specials.ToList();

            return View(specials);
        }

        [HttpGet]
        public ActionResult Contact(string VIN)
        {
            ViewBag.Message = "Want to know more about a certain car you have your eye on?";
            var model = new ContactVM();
            model.VIN = VIN;

            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactVM contact)
        {
            var repo = new CarDealershipContext();

            if (ModelState.IsValid)
            {
                var newContact = new Contact
                {
                    ContactName = contact.ContactName,
                    ContactEmail = contact.ContactEmail,
                    ContactPhone = contact.ContactPhone,
                    ContactMessage = contact.ContactMessage,
                    VIN_ = contact.VIN,
                    ContactDate = DateTime.Now
                };

                repo.Contacts.Add(newContact);
                repo.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(contact);
        }
    }
}