using CarDealership.Data;
using CarDealership.UI.Models;
using CarDealership.UI.Models.Identity;
using CarDealership.UI.Models.MyViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealership.UI.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        ICarRepo repo = CarRepoFactory.CreateRepo();

        // GET: Vehicle
        public ActionResult Vehicles()
        {
            return View(repo.GetAllCars());
        }

        [HttpGet]
        public ActionResult AddVehicle()
        {
            var model = new AddEditVehicleVM();
            model.SetListItems();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddVehicle(AddEditVehicleVM pendingVehicle) //Trouble getting the model info
        {
            if (ModelState.IsValid)
            {
                if(pendingVehicle.ImageFile != null && pendingVehicle.ImageFile.ContentLength > 0)
                {
                    var newVehicle = new Car();
                    newVehicle.Model = repo.GetAllModels().Single(m => m.ModelName == pendingVehicle.SelectedModel && m.ModelEdition == pendingVehicle.SelectedEdition);
                    newVehicle.ModelId = newVehicle.Model.ModelId;
                    newVehicle.CarType = pendingVehicle.SelectedType;
                    newVehicle.VIN_ = pendingVehicle.FormVIN;
                    newVehicle.BodyStyle = repo.GetBodyStyle(pendingVehicle.SelectedBodyStyle);
                    newVehicle.BSId = newVehicle.BodyStyle.BSId;
                    newVehicle.CarYear = pendingVehicle.Year.Value;
                    newVehicle.CarDescription = pendingVehicle.Description;
                    newVehicle.ExteriorColor = repo.GetExtColor(pendingVehicle.SelectedColor, pendingVehicle.SelectedColorType);
                    newVehicle.ExtColorId = newVehicle.ExteriorColor.ExtColorId;
                    newVehicle.InteriorColor = repo.GetIntColor(pendingVehicle.SelectedIntColor, pendingVehicle.SelectedIntColorType);
                    newVehicle.IntColorId = newVehicle.InteriorColor.IntColorId;
                    newVehicle.Mileage = pendingVehicle.Mileage.Value;
                    newVehicle.MSRP = pendingVehicle.MSRP.Value;
                    newVehicle.SalePrice = pendingVehicle.SalePrice.Value;
                    newVehicle.Transmission = repo.GetTransmission(pendingVehicle.SelectedTransmision);
                    newVehicle.TransmissionId = newVehicle.Transmission.TransmissionId;
                    newVehicle.Purchased = "N";
                    newVehicle.Featured = "N";
                    newVehicle.CarPicture = pendingVehicle.ImageFile.FileName;
                    string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(pendingVehicle.ImageFile.FileName));
                    pendingVehicle.ImageFile.SaveAs(path);

                    repo.AddCar(newVehicle);
                    return RedirectToAction("Vehicles");
                }
                else
                {
                    ModelState.AddModelError("", "You must upload an image file in one of the following formats: .jpg|.jpeg|.png");
                    pendingVehicle.ResetModelsAndEditions(pendingVehicle.SelectedMake, pendingVehicle.SelectedModel);
                    pendingVehicle.SetListItems();
                    return View(pendingVehicle);
                }
            }

            pendingVehicle.SetListItems();
            pendingVehicle.ResetModelsAndEditions(pendingVehicle.SelectedMake, pendingVehicle.SelectedModel);
            return View(pendingVehicle);
        }

        [HttpGet]
        public ActionResult EditVehicle(string VIN)
        {
            var model = new AddEditVehicleVM();
            model.SetListItems();
            var vehicle = repo.Get(VIN);
            model.VIN = vehicle.VIN_;
            model.FormVIN = vehicle.VIN_;
            model.SelectedMake = vehicle.Model.Make.MakeName;
            model.SelectedModel = vehicle.Model.ModelName;
            model.SelectedEdition = vehicle.Model.ModelEdition;
            model.SelectedColor = vehicle.ExteriorColor.ExtColorName;
            model.SelectedColorType = vehicle.ExteriorColor.ExtColorType;
            model.SelectedIntColor = vehicle.InteriorColor.IntColorName;
            model.SelectedIntColorType = vehicle.InteriorColor.IntColorType;
            model.SelectedBodyStyle = vehicle.BodyStyle.BSName;
            model.SelectedType = vehicle.CarType;
            model.SelectedTransmision = vehicle.Transmission.TransmissionType;
            model.Description = vehicle.CarDescription;
            if(vehicle.Featured == "Y") model.Featured = true;
            model.Mileage = vehicle.Mileage;
            model.MSRP = vehicle.MSRP;
            model.SalePrice = vehicle.SalePrice;
            model.Year = vehicle.CarYear;
            model.CarPicture = vehicle.CarPicture;
            model.Purchased = vehicle.Purchased;

            model.ResetModelsAndEditions(model.SelectedMake, model.SelectedModel);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditVehicle(AddEditVehicleVM vehicle)
        {
            if (ModelState.IsValid)
            {
                var updatedVehicle = new Car();
                updatedVehicle.Model = repo.GetAllModels().Single(m => m.ModelName == vehicle.SelectedModel && m.ModelEdition == vehicle.SelectedEdition);
                updatedVehicle.ModelId = updatedVehicle.Model.ModelId;
                updatedVehicle.CarType = vehicle.SelectedType;
                updatedVehicle.BodyStyle = repo.GetBodyStyle(vehicle.SelectedBodyStyle);
                updatedVehicle.BSId = updatedVehicle.BodyStyle.BSId;
                updatedVehicle.CarPicture = vehicle.CarPicture;
                updatedVehicle.CarYear = vehicle.Year.Value;
                updatedVehicle.CarDescription = vehicle.Description;
                updatedVehicle.ExteriorColor = repo.GetExtColor(vehicle.SelectedColor, vehicle.SelectedColorType);
                updatedVehicle.ExtColorId = updatedVehicle.ExteriorColor.ExtColorId;
                updatedVehicle.InteriorColor = repo.GetIntColor(vehicle.SelectedIntColor, vehicle.SelectedIntColorType);
                updatedVehicle.IntColorId = updatedVehicle.InteriorColor.IntColorId;
                updatedVehicle.Mileage = vehicle.Mileage.Value;
                updatedVehicle.MSRP = vehicle.MSRP.Value;
                updatedVehicle.SalePrice = vehicle.SalePrice.Value;
                updatedVehicle.Transmission = repo.GetTransmission(vehicle.SelectedTransmision);
                updatedVehicle.TransmissionId = updatedVehicle.Transmission.TransmissionId;
                updatedVehicle.Purchased = vehicle.Purchased;
                updatedVehicle.Featured = (vehicle.Featured) ? "Y" : "N";

                if (vehicle.ImageFile != null && vehicle.ImageFile.ContentLength > 0)
                {
                    updatedVehicle.CarPicture = vehicle.ImageFile.FileName;
                    string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(vehicle.ImageFile.FileName));
                    vehicle.ImageFile.SaveAs(path);
                }

                if(vehicle.VIN != vehicle.FormVIN)
                {
                    repo.DeleteCar(vehicle.VIN);
                    updatedVehicle.VIN_ = vehicle.FormVIN;
                    repo.AddCar(updatedVehicle);
                }
                else
                {
                    updatedVehicle.VIN_ = vehicle.VIN;
                    repo.EditCar(updatedVehicle);
                }
                
                return RedirectToAction("Vehicles");
            }

            vehicle.SetListItems();
            vehicle.ResetModelsAndEditions(vehicle.SelectedMake, vehicle.SelectedModel);
            return View(vehicle);
        }

        [HttpGet]
        public ActionResult DeleteVehicle(string VIN)
        {
            return View(repo.Get(VIN));
        }

        [HttpPost]
        public ActionResult DeleteVehicle(Car vehicle)
        {
            repo.DeleteCar(vehicle.VIN_);
            return RedirectToAction("Vehicles");
        }

        public ActionResult Users()
        {
            var model = new DisplayUsersVM();
            model.AllUsers = GetAllUsers();
            model.AllRoles = GetAllRoles();
            return View(model);
        }

        private List<SelectListItem> GetAllRoles()
        {
            CarDealershipDbContext context = new CarDealershipDbContext();
            var roleMgr = new RoleManager<AppRole>(new RoleStore<AppRole>(context));
            List<SelectListItem> toReturn = context.Roles.Select(appRole => new SelectListItem { Text = appRole.Name, Value = appRole.Name }).ToList();
            return toReturn;
        }

        private List<UserVM> GetAllUsers()
        {

            CarDealershipDbContext context = new CarDealershipDbContext();
            var roleMgr = new RoleManager<AppRole>(new RoleStore<AppRole>(context));
            List<UserVM> toReturn = context.Users.ToList().Select(appUser => new UserVM {
                Username = appUser.UserName,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                ID = appUser.Id,
                Email = appUser.Email,
                SelectedRole = roleMgr.FindById(appUser.Roles.FirstOrDefault().RoleId).Name }).ToList();
            return toReturn;

            //List<UserVM> toReturn;
            //using (CarDealershipDbContext context = new CarDealershipDbContext())
            //{
            //    var roleMgr = new RoleManager<AppRole>(new RoleStore<AppRole>(context));
            //    //List<UserVM> toReturn = context.Users.Select(appUser => new UserVM {
            //    //    Username = appUser.UserName,
            //    //    FirstName = appUser.FirstName,
            //    //    LastName = appUser.LastName,
            //    //    ID = appUser.Id,
            //    //    Email = appUser.Email,
            //    //    Role = roleMgr.FindById(appUser.Roles.First().RoleId).Name }).ToList();
            //    toReturn = context.Users.Select(appUser => new UserVM
            //    {
            //        Username = appUser.UserName,
            //        FirstName = appUser.FirstName,
            //        LastName = appUser.LastName,
            //        ID = appUser.Id,
            //        Email = appUser.Email
            //    }).ToList();
            //}

            //CarDealershipDbContext context2 = new CarDealershipDbContext();
            //var roleMgr2 = new RoleManager<AppRole>(new RoleStore<AppRole>(context2));
            //int i = 0;
            //foreach (var user in context2.Users)
            //{
            //    toReturn[i].Role = roleMgr2.FindById(user.Roles.First().RoleId).Name;
            //    i++;
            //}
            //return toReturn;
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            var model = new UserVM();
            model.AllRoles = GetAllRoles();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddUser(UserVM model)
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<AppUser>>();
                var user = new AppUser { UserName = model.Username, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email };
                var result = userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    var newUser = userManager.FindByName(model.Username);
                    result = userManager.AddToRole(newUser.Id, model.SelectedRole);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Users");
                    }
                }
            }
            
            model.AllRoles = GetAllRoles();
            return View(model);
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<AppUser>>();
            CarDealershipDbContext context = new CarDealershipDbContext();
            var roleMgr = new RoleManager<AppRole>(new RoleStore<AppRole>(context));
            var userToEdit = userManager.FindById(id);
            var model = new EditUserVM
            {
                Username = userToEdit.UserName,
                FirstName = userToEdit.FirstName,
                LastName = userToEdit.LastName,
                Email = userToEdit.Email,
                ID = userToEdit.Id,
                SelectedRole = roleMgr.FindById(userToEdit.Roles.First().RoleId).Name,
                CurrentRole = roleMgr.FindById(userToEdit.Roles.First().RoleId).Name
            };
            model.AllRoles = GetAllRoles();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditUser(EditUserVM editedUser, string id, string password)
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<AppUser>>();
                var oldUser = userManager.FindById(id);

                if (!string.IsNullOrEmpty(password))
                {
                    PasswordHasher hasher = new PasswordHasher();
                    oldUser.PasswordHash = hasher.HashPassword(password);
                }

                if (editedUser.SelectedRole != editedUser.CurrentRole)
                {
                    userManager.RemoveFromRole(oldUser.Id, editedUser.CurrentRole);
                    userManager.AddToRole(oldUser.Id, editedUser.SelectedRole);
                }

                oldUser.UserName = editedUser.Username;
                oldUser.FirstName = editedUser.FirstName;
                oldUser.LastName = editedUser.LastName;
                oldUser.Email = editedUser.Email;
                userManager.Update(oldUser);

                return RedirectToAction("Users");
            }

            editedUser.AllRoles = GetAllRoles();
            return View(editedUser);
        }

        [HttpGet]
        public ActionResult Makes()
        {
            var model = new Make();
            return View(model);
        }

        [HttpPost]
        public ActionResult Makes(Make make)
        {
            List<string> makes = repo.GetAllMakes().Select(m => m.MakeName.ToLower()).ToList();
            if (makes.Contains(make.MakeName.ToLower()))
            {
                ModelState.AddModelError("", $"The make '{make.MakeName}' already exists in the system.");
                return View(make);
            }

            if (ModelState.IsValid)
            {
                make.DateAdded = DateTime.Now;
                repo.AddMake(make);
                return RedirectToAction("Makes");
            }

            return View(make);
        }

        [HttpGet]
        public ActionResult Models()
        {
            var model = new AddModelVM();
            model.SetMakeItems(repo.GetAllMakes());
            return View(model);
        }

        [HttpPost]
        public ActionResult Models(AddModelVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.ModelEdition.Contains(","))
                {
                    string[] editions = model.ModelEdition.Split(',');

                    foreach(var edition in editions)
                    {
                        var newEdition = new Model()
                        {
                            Make = repo.GetAllMakes().First(m => m.MakeId.ToString() == model.SelectedMake),
                            ModelName = model.ModelName,
                            ModelEdition = edition,
                            DateAdded = DateTime.Now,
                            ModelUser = User.Identity.Name
                        };

                        repo.AddModel(newEdition);
                    }

                    return RedirectToAction("Models");
                }

                var newModel = new Model()
                {
                    Make = repo.GetAllMakes().First(m => m.MakeId.ToString() == model.SelectedMake),
                    ModelName = model.ModelName,
                    ModelEdition = model.ModelEdition,
                    DateAdded = DateTime.Now,
                    ModelUser = User.Identity.Name
                };
                repo.AddModel(newModel);
                return RedirectToAction("Models");
            }

            model.SetMakeItems(repo.GetAllMakes());
            return View(model);
        }

        [HttpGet]
        public ActionResult Specials()
        {
            var model = new SpecialVM();
            model.AllSpecials = repo.GetAllSpecials();
            return View(model);
        }

        [HttpPost]
        public ActionResult Specials(SpecialVM special)
        {
            if (ModelState.IsValid)
            {
                Special newSpecial = new Special
                {
                    SpecialName = special.SpecialName,
                    SpecialDescription = special.SpecialDescription,
                    SpecialStartDate = special.SpecialStartDate,
                    SpecialEndDate = special.SpecialEndDate
                };

                if (special.SpecialJTronImage != null && special.SpecialJTronImage.ContentLength > 0)
                {
                    newSpecial.SpecialJTronImage = special.SpecialJTronImage.FileName;
                    string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(special.SpecialJTronImage.FileName));
                    special.SpecialJTronImage.SaveAs(path);
                }

                repo.AddSpecial(newSpecial);
                return RedirectToAction("Specials");
            }

            return View(special);
        }

        [HttpGet]
        public ActionResult EditSpecial(int id)
        {
            var specialToEdit = repo.GetAllSpecials().Single(s => s.SpecialId == id);
            var model = new SpecialVM
            {
                SpecialId = specialToEdit.SpecialId,
                SpecialName = specialToEdit.SpecialName,
                SpecialDescription = specialToEdit.SpecialDescription,
                SpecialStartDate = specialToEdit.SpecialStartDate,
                SpecialEndDate = specialToEdit.SpecialEndDate,
                SpecialPicture = specialToEdit.SpecialJTronImage
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditSpecial(SpecialVM special)
        {
            if (ModelState.IsValid)
            {
                var updatedSpecial = new Special
                {
                    SpecialId = special.SpecialId,
                    SpecialName = special.SpecialName,
                    SpecialDescription = special.SpecialDescription,
                    SpecialStartDate = special.SpecialStartDate,
                    SpecialEndDate = special.SpecialEndDate,
                    SpecialJTronImage = special.SpecialPicture
                };

                if (special.SpecialJTronImage != null && special.SpecialJTronImage.ContentLength > 0)
                {
                    updatedSpecial.SpecialJTronImage = special.SpecialJTronImage.FileName;
                    string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(special.SpecialJTronImage.FileName));
                    special.SpecialJTronImage.SaveAs(path);
                }

                repo.EditSpecial(updatedSpecial);
                return RedirectToAction("Specials");
            }

            return View(special);
        }

        [HttpGet]
        public ActionResult DeleteSpecial(int id)
        {
            var specialToDelete = repo.GetAllSpecials().Single(s => s.SpecialId == id);

            return View(specialToDelete);
        }

        [HttpPost]
        public ActionResult DeleteSpecial(Special special)
        {
            repo.DeleteSpecial(special.SpecialId);

            return RedirectToAction("Specials");
        }
    }
}