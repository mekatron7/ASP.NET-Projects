using ChromebookManager.Data;
using ChromebookManager.Models;
using ChromebookManager.Models.Admin;
using ChromebookManager.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChromebookManager.Controllers
{
    public class HomeController : Controller
    {
        private CBRepo _repo = new CBRepo();
        public static string _user = "Admin";
        private static List<Alert> _alerts = new List<Alert>();
        private static List<Inventory> _addPartsUsedInventory;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string navSearch)
        {
            //return Redirect("/ExcelFiles/Test.xlsx");
            return View("Index");
        }

        public ActionResult RepairRequest(string userName = null)
        {
            var model = new RepairRequestVM { Schools = SetSchools(), IssueTypes = _repo.GetIssueTypes(), StudentUsername = userName };
            if (!string.IsNullOrEmpty(userName)) model.FromClientProfile = true;
            return View(model);
        }

        [HttpPost]
        public ActionResult RepairRequest(RepairRequestVM model)
        {
            //1. Get device information and check if barcode is valid
            //2. Check if client is valid
            //3. Check if a new client has to be added and retrieve client-school-id
            //4. If not, check if client has to be added to school and retrieve client-school-id
            //5. If not, retrieve client-school-id
            //5. If device isn't in system, add to system and retrieve client-device-id
            //6. If device is in system, but not linked to client, add to client-device table and retrieve client-device-id
            //7. If device is in system and linked to client, retrieve device and get the client-device-id
            //8. Add new repair-log entry using the newly generated or retrieved client-device-id

            //Verify if barcode is valid. Return error alert if it's invalid.
            //var newDevice = new Device { Barcode = model.Barcode, SerialNumber = model.SerialNumber, AddedBy = _user, ModelId = 1 };
            //var device = _repo.GetDevice(model.Barcode);
            int cdid;
            int clientId = 0;
            bool isNewClient = false;
            Client retrievedClient;

            if (model.Unassigned)
            {
                retrievedClient = _repo.GetUnassignedClient();
                if (retrievedClient == null) clientId = _repo.AddUnassignedClient(_user);
                else clientId = retrievedClient.ClientId;
            }
            else
            {
                retrievedClient = _repo.GetClientByUsername(model.StudentUsername);
                if (retrievedClient == null)
                {
                    Client newClient;
                    newClient = _repo.AddClientAutomatic(new Client { Username = model.StudentUsername, AddedBy = _user });
                    model.SchoolId = newClient.SchoolId;
                    isNewClient = true;
                    if (newClient.DoesNotExist)
                    {
                        CreateAlert(model, "Whoops!", $"There's no user in the district with a username of {model.StudentUsername}.", "alert-danger");
                        return View(model);
                    }
                    if (newClient.NotEnrolled)
                    {
                        CreateAlert(model, "Whoops!", $"This student is not currently enrolled in the district.", "alert-warning");
                        return View(model);
                    }
                    clientId = newClient.ClientId;
                }
                else clientId = retrievedClient.ClientId;
            }
            Device deviceFromDestiny;
            if (model.Unassigned)
            {
                deviceFromDestiny = _repo.GetUnassignedDeviceInfo(model.Barcode);

                if(deviceFromDestiny == null)
                {
                    CreateAlert(model, "Whoops!", $"The barcode {model.Barcode} either doesn't exist or isn't linked to a device in the system.", "alert-danger");
                    return View(model);
                }
                if (!string.IsNullOrEmpty(deviceFromDestiny.CurrentOwner))
                {
                    CreateAlert(model, "Whoops!", $"This device is currently assigned to {deviceFromDestiny.CurrentOwner}.", "alert-warning");
                    return View(model);
                }
            }
            else
            {
                deviceFromDestiny = _repo.GetStudentDeviceInfo(model.StudentUsername);

                if(deviceFromDestiny == null)
                {
                    CreateAlert(model, "Whoops!", $"The user {model.StudentUsername} currently has no chromebook assigned to them.", "alert-danger");
                    if (isNewClient) _repo.DeleteClient(clientId);
                    return View(model);
                }
            }

            var clientDevices = _repo.GetDevicesByClient(clientId);
            if (clientDevices.Any(d => d.Loaner)) cdid = clientDevices.First(d => d.Loaner && !d.EndDate.HasValue).ClientDeviceId;
            else
            {
                var savedDevice = _repo.GetDevice(deviceFromDestiny.Barcode);
                SetUpModelInfo(deviceFromDestiny);
                if (savedDevice == null) cdid = _repo.AddDevice(deviceFromDestiny, clientId, model.SchoolId);
                else if (!clientDevices.Any(d => d.Barcode == deviceFromDestiny.Barcode)) cdid = _repo.AddClientDevice(clientId, model.SchoolId, deviceFromDestiny.Barcode, _user);
                else cdid = clientDevices.First(d => d.SchoolId == model.SchoolId).ClientDeviceId;
            }
            _repo.AddRepairLog(new RepairLog { ClientDeviceId = cdid, IssueId = model.IssueType, IssueDescription = model.IssueDescription, EmailAddress = "admin@rdale.org", AddedBy = _user });

            return RedirectToAction("RepairLog");
        }

        public ActionResult RepairLog()
        {
            return View(_repo.GetRepairLogs());
        }

        public ActionResult RepairDetails(int id)
        {
            var details = _repo.GetRepairLog(id);
            var client = _repo.GetClientByUsername(details.Username);
            if (client == null) client = _repo.GetUnassignedClient();
            var partsUsed = _repo.GetPartsUsed(id);
            var partsSelectList = new List<SelectListItem>();
            var schoolInv = _repo.GetInventoryBySchool(details.SchoolId);
            _addPartsUsedInventory = schoolInv;
            foreach (var inv in schoolInv.ToList())
            {
                if(inv.RecycledQty > 0)
                {
                    var recycledInv = inv.Copy();
                    recycledInv.Qty = recycledInv.RecycledQty;
                    recycledInv.PartName += " (Recycled)";
                    _addPartsUsedInventory.Add(recycledInv);
                }
            }
            _addPartsUsedInventory.RemoveAll(i => i.Qty == 0);
            var device = _repo.GetDevice(details.Barcode);
            var modelName = $"{device.BrandName} {device.ModelNumber}";

            foreach (var inv in _addPartsUsedInventory)
            {
                var invId = inv.PartName.Contains("(Recycled)") ? $"{inv.InventoryId}R" : $"{inv.InventoryId}";
                partsSelectList.Add(new SelectListItem { Text = $"{inv.BrandName} {inv.ModelNumber} {inv.PartName}", Value = invId });
            }
                
            var model = new RepairDetailsVM { RepairLog = details, Client = client, PartsUsed = partsUsed, PartsSelectList = partsSelectList, Model = modelName };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditRepair(RepairDetailsVM model)
        {
            var log = new RepairLog
            {
                RepairId = model.RepairLog.RepairId,
                RepairNotes = model.RepairLog.RepairNotes,
                Notes = model.RepairLog.Notes,
                RepairReturnedDate = model.RepairLog.RepairReturnedDate,
                WarrantyRepairSentDate = model.RepairLog.WarrantyRepairSentDate
            };
            _repo.EditRepairLog(log);
            return RedirectToAction("RepairLog");
        }

        public ActionResult DeleteRepairLog(int repairIdDelete)
        {
            var deleted = _repo.DeleteRepairLog(repairIdDelete);
            return RedirectToAction("RepairLog");
        }

        public ActionResult Clients()
        {
            return View(_repo.GetAllClients());
        }

        public ActionResult ClientProfile(int id)
        {
            var model = new ClientProfileVM
            {
                ClientInfo = _repo.GetClient(id),
                RepairLogs = _repo.GetRepairLogByClient(id),
                PartsUsed = _repo.GetPartsUsedByClient(id),
                Devices = _repo.GetDevicesByClient(id)
            };
            return View(model);
        }

        public ActionResult Devices()
        {
            return View(_repo.GetAllDevices());
        }

        public ActionResult Inventory()
        {
            var model = new InventoryVM();
            model.InventoryList = _repo.GetAllInventory();
            model.SchoolSelectList = SetSchools();
            var modelPartsList = new List<SelectListItem>();
            foreach (var mp in _repo.GetModelParts()) modelPartsList.Add(new SelectListItem { Text = $"{mp.BrandName} {mp.ModelNumber} {mp.PartName}", Value = mp.ModelPartId.ToString() });
            model.ModelPartSelectList = modelPartsList;
            model.Alerts.AddRange(_alerts);
            _alerts.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddInventory(InventoryVM model)
        {
            var modelPart = _repo.GetModelParts().First(m => m.ModelPartId == model.ModelPartId);
            var schoolName = _repo.GetSchools().First(s => s.SchoolId == model.SchoolId).SchoolName;
            var pluralString = model.Qty > 1 ? "s have" : " has";
            var sAtEnd = schoolName.Last() == 's' ? "'" : "'s";
            if(model.Qty > 1 && modelPart.PartName.Last() == 'y')
            {
                modelPart.PartName = modelPart.PartName.Remove(modelPart.PartName.Length - 1) + "ies";
                pluralString = " have";
            }
            _repo.AddInventory(new Inventory
            {
                ModelPartId = model.ModelPartId,
                SchoolId = model.SchoolId,
                Qty = model.Qty,
                LastModifiedBy = _user
            }, model.Recycled);
            var alert = CreateAlert("Success!", $"{model.Qty} {modelPart.BrandName} {modelPart.ModelNumber} {(model.Recycled ? "recycled " : "")}{modelPart.PartName}{pluralString} been added to {schoolName}{sAtEnd} inventory.", "alert-success");
            _alerts.Add(alert);
            return RedirectToAction("Inventory");
        }

        [HttpPost]
        public ActionResult EditInventory(InventoryVM model)
        {
            var inv = _repo.GetInventory(model.InventoryId);
            var schoolName = inv.SchoolName;
            var partName = $"{inv.BrandName} {inv.ModelNumber} {inv.PartName}";
            _repo.EditInventory(model.InventoryId, model.Qty, model.RecycledQty, model.Notes, _user);
            var alert = CreateAlert("Success!", $"The quantity of {partName} for {schoolName} has been changed to {model.Qty}.", "alert-success");
            _alerts.Add(alert);
            if(model.RecycledQty != inv.RecycledQty)
            {
                alert = CreateAlert("Success!", $"The quantity of {partName} (recycled) for {schoolName} has been changed to {model.RecycledQty}.", "alert-success");
                _alerts.Add(alert);
            }
            return RedirectToAction("Inventory");
        }

        [HttpPost]
        public ActionResult TransferInventory(InventoryVM model)
        {
            var pluralString = model.Qty > 1 ? "s have" : " has";
            if (model.PartName.Last() == 'y')
            {
                model.PartName = model.PartName.Remove(model.PartName.Length - 1) + "ies";
                pluralString = " have";
            }
            _repo.TransferInventory(model.InventoryId, model.SchoolId, model.Qty, _user, model.Recycled);
            var alert = CreateAlert("Success!", $"{model.Qty} {(model.Recycled ? "recycled " : "")}{model.PartName}{pluralString} been transferred from {model.FromSchool} to {model.ToSchool}.", "alert-success");
            _alerts.Add(alert);

            return RedirectToAction("Inventory");
        }

        [HttpPost]
        public ActionResult AddPartsUsed(RepairDetailsVM model)
        {
            foreach (var inv in model.AddedPartsUsed)
            {
                bool recycled = inv.Contains('R');
                var invId = recycled ? int.Parse(inv.Substring(0, inv.Length - 1)) : int.Parse(inv);

                _repo.AddPartUsed(model.RepairLog.RepairId, invId, recycled);
            }
            return RedirectToAction("RepairDetails", new { id = model.RepairLog.RepairId });
        }

        [HttpPost]
        public ActionResult RemovePartUsed(int partUsedId, int repairId)
        {
            _repo.RemovePartUsed(partUsedId);
            return RedirectToAction("RepairDetails", new { id = repairId });
        }

        public ActionResult Notifications()
        {
            var notifications = _repo.GetNotifications(_user);
            foreach(var notif in notifications)
            {
                if(notif.NotifType == "InvRequest")
                {
                    int? qty = _repo.GetInventory(notif.ModelPartId.Value, 7)?.Qty;
                    if (notif.Qty.Value <= qty)
                        notif.Fulfillable = true;
                }
                else if(notif.NotifType == "InvRequestRec")
                {
                    int? qty = _repo.GetInventory(notif.ModelPartId.Value, 7)?.RecycledQty;
                    if (notif.Qty.Value <= qty)
                        notif.Fulfillable = true;
                }
            }
            _repo.NotificationsSeen(_user);
            return View(notifications.OrderByDescending(n => n.NotifDate).ToList());
        }

        public ActionResult DeleteNotification(int id)
        {
            _repo.DeleteNotification(id);
            return RedirectToAction("Notifications");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        public ActionResult SetNotificationsNumber()
        {
            ViewBag.NotifNumber = _repo.GetNotifications(_user).Where(n => !n.Seen).Count();
            return PartialView("_SetNotificationsNumber");
        }

        public ActionResult AddNewPartUsed(int index, string prevInvId)
        {
            var partChosen = false;
            bool recycled = prevInvId.Contains('R');
            int prevInvIdInt = recycled ? int.Parse(prevInvId.Remove(prevInvId.Length - 1)) : int.Parse(prevInvId);
            var prevInv = recycled ? _addPartsUsedInventory.FirstOrDefault(i => i.InventoryId == prevInvIdInt && i.PartName.Contains("(Recycled)")) :
                _addPartsUsedInventory.FirstOrDefault(i => i.InventoryId == prevInvIdInt && !i.PartName.Contains("(Recycled)"));
            if (prevInv != null)
            {
                partChosen = true;
                prevInv.Qty--;
                int qtyCheck = _addPartsUsedInventory.First(i => i.InventoryId == prevInvIdInt).Qty;
                if (prevInv.Qty == 0) _addPartsUsedInventory.Remove(prevInv);
            }
            var inventoryList = new List<SelectListItem>();
            if (partChosen)
            {
                foreach (var inv in _addPartsUsedInventory)
                {
                    var invId = inv.PartName.Contains("(Recycled)") ? $"{inv.InventoryId}R" : $"{inv.InventoryId}";
                    inventoryList.Add(new SelectListItem
                    {
                        Text = $"{inv.BrandName} {inv.ModelNumber} {inv.PartName}",
                        Value = invId
                    });
                }
            }

            var model = new AddNewPartUsedVM { Index = index, InventoryList = inventoryList, PartChosen = partChosen };
            return PartialView("_AddNewPartUsed", model);
        }

        public List<SelectListItem> SetSchools()
        {
            var schools = new List<SelectListItem>();
            foreach (var school in _repo.GetSchools()) schools.Add(new SelectListItem { Value = school.SchoolId.ToString(), Text = school.SchoolName });
            return schools;
        }

        public void CreateAlert(RepairRequestVM model, string alertTitle, string alertMessage, string alertType)
        {
            var alert = new Alert
            {
                AlertTitle = alertTitle,
                AlertMessage = alertMessage,
                AlertType = alertType
            };
            model.Alert = alert;
            model.Schools = SetSchools();
            model.IssueTypes = _repo.GetIssueTypes();
        }

        public Alert CreateAlert(string alertTitle, string alertMessage, string alertType)
        {
            var alert = new Alert
            {
                AlertTitle = alertTitle,
                AlertMessage = alertMessage,
                AlertType = alertType
            };
            return alert;
        }

        public void SetUpModelInfo(Device device)
        {
            var modelName = device.ModelNumber;
            var modelInfo = modelName.Split(' ');
            string brand = modelInfo[0];
            string modelNumber = null;
            string storageCapacity = null;
            for (int i = 1; i < modelInfo.Length; i++)
            {
                var info = modelInfo[i];
                if (info.Contains("GB")) storageCapacity = info;
                else
                {
                    bool hasNumber = false;
                    for (int j = 0; j < info.Length; j++)
                    {
                        if (char.IsNumber(info[j]))
                        {
                            hasNumber = true;
                            break;
                        }
                    }
                    if(hasNumber || info.Length < 3) modelNumber += info + " ";
                }
            }
            modelNumber = modelNumber.Trim();
            var brands = _repo.GetBrands();
            var retrievedBrand = brands.FirstOrDefault(b => b.BrandName.ToLower() == brand.ToLower());
            if (retrievedBrand == null)
                _repo.AddBrand(new Brand { BrandName = brand, AddedBy = "System" });
            int brandId = _repo.GetBrands().First(b => b.BrandName.ToLower() == brand.ToLower()).BrandId;
            var models = _repo.GetModels();
            var retrievedModel = models.FirstOrDefault(m => m.BrandName.ToLower() == brand.ToLower() && m.ModelNumber == modelNumber);
            if (retrievedModel == null)
                _repo.AddModel(new Model { ModelNumber = modelNumber, BrandId = brandId, AddedBy = "System", Cost = device.Price });
            int modelId = _repo.GetModels().First(m => m.ModelNumber == modelNumber && m.BrandName.ToLower() == brand.ToLower()).ModelId;
            device.ModelId = modelId;
            device.StorageCapacity = storageCapacity;
            device.AddedBy = _user;
        }
    }
}