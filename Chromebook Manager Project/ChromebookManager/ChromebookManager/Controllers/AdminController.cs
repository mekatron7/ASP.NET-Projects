using ChromebookManager.Data;
using ChromebookManager.Models;
using ChromebookManager.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChromebookManager.Controllers
{
    public class AdminController : Controller
    {
        private CBRepo _repo = new CBRepo();
        // GET: Admin
        private static Alert _alert;
        private string _user = HomeController._user;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SysData()
        {
            var model = new SysDataVM();
            model.Brands = _repo.GetBrands();
            model.Models = _repo.GetModels();
            model.Parts = _repo.GetParts();
            model.IssueTypes = _repo.GetIssueTypes();
            model.ModelParts = _repo.GetModelParts();
            model.Alert = _alert;
            var brandList = new List<SelectListItem>();
            foreach (var brand in model.Brands) brandList.Add(new SelectListItem { Value = brand.BrandId.ToString(), Text = brand.BrandName });
            var modelList = new List<SelectListItem>();
            foreach (var theModel in model.Models) modelList.Add(new SelectListItem { Value = theModel.ModelId.ToString(), Text = $"{theModel.BrandName} {theModel.ModelNumber}" });
            var partList = new List<SelectListItem>();
            foreach (var part in model.Parts) partList.Add(new SelectListItem { Value = part.PartId.ToString(), Text = part.PartName });
            var purchaseOrders = new List<SelectListItem>();
            foreach (var po in _repo.GetPurchaseOrders().Where(po => po.NumOfPendingLIs > 0))
                purchaseOrders.Add(new SelectListItem { Value = po.PONumber.ToString(), Text = $"PO #: {po.PONumber} | User: {po.Username}" });
            model.PurchaseOrders = purchaseOrders;
            model.BrandSelectList = brandList;
            model.ModelSelectList = modelList;
            model.PartSelectList = partList;
            _alert = null;

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateBrand(string brandName)
        {
            var brand = _repo.GetBrands().FirstOrDefault(b => b.BrandName.ToLower() == brandName.ToLower());
            var alert = new Alert();
            if(brand != null)
            {
                alert.AlertMessage = $"The brand {brand.BrandName} already exists.";
                alert.AlertTitle = "Whoops!";
                alert.AlertType = "alert-danger";
            }
            else
            {
                _repo.AddBrand(new Brand { BrandName = brandName, AddedBy = _user });
                alert.AlertMessage = $"The brand {brandName} has been added to the system.";
                alert.AlertTitle = "Success!";
                alert.AlertType = "alert-success";
            }
            _alert = alert;
            return RedirectToAction("SysData");
        }

        public ActionResult CreateModel(SysDataVM model)
        {
            var modelInfo = _repo.GetModels().FirstOrDefault(m => m.ModelNumber.ToLower() == model.ModelNumber.ToLower() && m.BrandId == model.BrandId);
            var alert = new Alert();
            if (modelInfo != null)
            {
                alert.AlertMessage = $"The model {modelInfo.BrandName} {modelInfo.ModelNumber} already exists.";
                alert.AlertTitle = "Whoops!";
                alert.AlertType = "alert-danger";
            }
            else
            {
                _repo.AddModel(new Model { ModelNumber = model.ModelNumber, BrandId = model.BrandId, Cost = model.Cost, AddedBy = _user });
                var brandName = _repo.GetBrands().First(b => b.BrandId == model.BrandId).BrandName;
                alert.AlertMessage = $"The model {brandName} {model.ModelNumber} has been added to the system.";
                alert.AlertTitle = "Success!";
                alert.AlertType = "alert-success";
            }
            _alert = alert;
            return RedirectToAction("SysData");
        }

        [HttpPost]
        public ActionResult CreatePart(string partName)
        {
            var part = _repo.GetParts().FirstOrDefault(p => p.PartName.ToLower() == partName.ToLower());
            var alert = new Alert();
            if (part != null)
            {
                alert.AlertMessage = $"The part {part.PartName} already exists.";
                alert.AlertTitle = "Whoops!";
                alert.AlertType = "alert-danger";
            }
            else
            {
                _repo.AddPart(new Part { PartName = partName, AddedBy = _user });
                alert.AlertMessage = $"The part {partName} has been added to the system.";
                alert.AlertTitle = "Success!";
                alert.AlertType = "alert-success";
            }
            _alert = alert;
            return RedirectToAction("SysData");
        }

        [HttpPost]
        public ActionResult CreateModelPart(SysDataVM model)
        {
            var mpInfoList = _repo.GetModelParts();
            var mpInfo = mpInfoList.FirstOrDefault(m => m.ModelId == model.ModelId && m.PartId == model.PartId);
            var alert = new Alert();
            if (mpInfo != null)
            {
                alert.AlertMessage = $"The model part {mpInfo.BrandName} {mpInfo.ModelNumber} {mpInfo.PartName} already exists.";
                alert.AlertTitle = "Whoops!";
                alert.AlertType = "alert-danger";
            }
            else
            {
                _repo.AddModelPart(new ModelPart { ModelId = model.ModelId, PartId = model.PartId, Cost = model.Cost, AddedBy = _user });
                var theModel = _repo.GetModels().First(m => m.ModelId == model.ModelId);
                var part = _repo.GetParts().First(p => p.PartId == model.PartId);
                alert.AlertMessage = $"The model part {theModel.BrandName} {theModel.ModelNumber} {part.PartName} has been added to the system.";
                alert.AlertTitle = "Success!";
                alert.AlertType = "alert-success";
            }
            _alert = alert;
            return RedirectToAction("SysData");
        }

        [HttpPost]
        public ActionResult CreateIssueType(string issueType, string issueDescription)
        {
            var issue = _repo.GetIssueTypes().FirstOrDefault(i => i.IssueName.ToLower() == issueType.ToLower());
            var alert = new Alert();
            if (issue != null)
            {
                alert.AlertMessage = $"The issue type '{issue.IssueName}' already exists.";
                alert.AlertTitle = "Whoops!";
                alert.AlertType = "alert-danger";
            }
            else
            {
                _repo.AddIssueType(new IssueType { IssueName = issueType, IssueDescription = issueDescription, AddedBy = _user });
                alert.AlertMessage = $"The issue type '{issueType}' has been added to the system.";
                alert.AlertTitle = "Success!";
                alert.AlertType = "alert-success";
            }
            _alert = alert;
            return RedirectToAction("SysData");
        }

        [HttpPost]
        public ActionResult EditModelCost(SysDataVM model)
        {
            var alert = new Alert();
            var theModel = _repo.GetModels().First(m => m.ModelId == model.ModelId);
            if (model.Cost == theModel.Cost)
            {
                alert.AlertMessage = $"The cost for {theModel.BrandName} {theModel.ModelNumber} is already set at {string.Format("{0:c}", theModel.Cost)}.";
                alert.AlertTitle = "Whoops!";
                alert.AlertType = "alert-warning";
            }
            else
            {
                _repo.EditModelCost(model.ModelId, model.Cost, _user);
                alert.AlertMessage = $"The cost for {theModel.BrandName} {theModel.ModelNumber} has been changed to {string.Format("{0:c}", model.Cost)}.";
                alert.AlertTitle = "Success!";
                alert.AlertType = "alert-success";
            }
            _alert = alert;
            return RedirectToAction("SysData");
        }

        [HttpPost]
        public ActionResult EditModelPart(SysDataVM model)
        {
            var alert = new Alert();
            var mp = _repo.GetModelParts().First(m => m.ModelPartId == model.ModelPartId);
            if(model.Cost == mp.Cost)
            {
                alert.AlertMessage = $"The cost for {mp.BrandName} {mp.ModelNumber} {mp.PartName} is already set at {string.Format("{0:c}", mp.Cost)}.";
                alert.AlertTitle = "Whoops!";
                alert.AlertType = "alert-warning";
            }
            else
            {
                _repo.EditModelPart(model.ModelPartId, model.Cost, _user);
                alert.AlertMessage = $"The cost for {mp.BrandName} {mp.ModelNumber} {mp.PartName} has been changed to {string.Format("{0:c}", model.Cost)}.";
                alert.AlertTitle = "Success!";
                alert.AlertType = "alert-success";
            }
            _alert = alert;
            return RedirectToAction("SysData");
        }

        [HttpPost]
        public ActionResult EditIssueType(string issueType, string issueDescription, int issueId)
        {
            var issueTypes = _repo.GetIssueTypes();
            var issue = issueTypes.FirstOrDefault(i => i.IssueId == issueId);
            var issue2 = issueTypes.FirstOrDefault(i => i.IssueName.ToLower() == issueType.ToLower());
            var alert = new Alert();
            if (issue2 != null && issue != issue2)
            {
                alert.AlertMessage = $"The issue type '{issue2.IssueName}' already exists.";
                alert.AlertTitle = "Whoops!";
                alert.AlertType = "alert-danger";
            }
            else
            {
                _repo.EditIssueType(new IssueType { IssueId = issueId, IssueName = issueType, IssueDescription = issueDescription });
                alert.AlertMessage = $"Changes to the issue type '{issueType}' have been saved.";
                alert.AlertTitle = "Success!";
                alert.AlertType = "alert-success";
            }
            _alert = alert;
            return RedirectToAction("SysData");
        }

        [HttpPost]
        public ActionResult DeleteBrand(int brandIdDelete)
        {
            var deleted = _repo.DeleteBrand(brandIdDelete);
            return RedirectToAction("SysData");
        }

        public ActionResult PurchaseOrders()
        {
            var model = new PurchaseOrderVM();
            var purchaseOrders = _repo.GetPurchaseOrders();
            model.PurchaseOrders = purchaseOrders;
            return View(model);
        }

        [HttpPost]
        public ActionResult AddPurchaseOrder(SysDataVM model)
        {
            if (model.SelectedPONumber == 0)
            {
                _repo.AddPurchaseOrder(new PurchaseOrder
                {
                    PONumber = model.PONumber,
                    MPCostId = model.MPCostId,
                    TotalQty = model.Qty,
                    Notes = model.Notes,
                    Username = _user
                }, model.ModelPartId);
            }
            else
            {
                //Make it so that it'll edit a LI if the mpCostId is the same
                //Add a new LI if the mpCostId is different
                var lineItems = _repo.GetPurchaseOrderLIs(model.SelectedPONumber);
                var li = lineItems.FirstOrDefault(l => l.ModelPartId == model.ModelPartId && l.MPCostId == model.MPCostId);
                if (li != null)
                    _repo.AddToPOLIQty(li.POLineItemId, model.Qty);
                else _repo.AddPurchaseOrderLI(new PurchaseOrderLI
                {
                    PONumber = model.SelectedPONumber,
                    ModelPartId = model.ModelPartId,
                    Qty = model.Qty,
                    MPCostId = model.MPCostId,
                    TotalPrice = model.TotalPrice.HasValue ? model.TotalPrice : null
                });
            }
            return RedirectToAction("PurchaseOrders");
        }

        public ActionResult ViewLineItems(long po)
        {
            var model = new PurchaseOrderVM();
            model.LineItems = _repo.GetPurchaseOrderLIs(po);
            model.PONumber = po;
            model.DateOrdered = _repo.GetPurchaseOrders().First(p => p.PONumber == po).TransactionDate;
            model.Alert = _alert;
            _alert = null;
            var partsList = new List<SelectListItem>();
            foreach (var part in _repo.GetModelParts())
                partsList.Add(new SelectListItem { Text = $"{part.BrandName} {part.ModelNumber} {part.PartName} | Cost: {string.Format("{0:c}", part.Cost)}", Value = part.ModelPartId.ToString() });
            model.PartsSelectList = partsList;
            return View(model);
        }

        public ActionResult AddToPurchaseOrder(PurchaseOrderVM model)
        {
            var part = _repo.GetModelParts().First(m => m.ModelPartId == model.SelectedPart);
            var lineItems = _repo.GetPurchaseOrderLIs(model.PONumber);
            var li = lineItems.FirstOrDefault(l => l.ModelPartId == part.ModelPartId && l.MPCostId == part.MPCostId);
            if (li != null)
                _repo.AddToPOLIQty(li.POLineItemId, model.AddToPOQty);
            else _repo.AddPurchaseOrderLI(new PurchaseOrderLI
            {
                PONumber = model.PONumber,
                ModelPartId = part.ModelPartId,
                Qty = model.AddToPOQty,
                MPCostId = part.MPCostId
            });
            return RedirectToAction("ViewLineItems", new { po = model.PONumber });
        }

        public ActionResult EditLI(PurchaseOrderVM model)
        {
            var li = _repo.GetPurchaseOrderLI(model.POLineItemId);
            var nhlcSchId = 7;
            var nhlcInv = _repo.GetInventory(li.ModelPartId, nhlcSchId);
            var alert = new Alert();
            if (li.DateReceived.HasValue && !model.DateReceived.HasValue && !model.TotalPrice.HasValue)
            {
                if(nhlcInv == null || nhlcInv.Qty < li.Qty)
                {
                    alert.AlertMessage = $"Inventory at NHLC either doesn't exist, or is too low to clear the Date Received field for PO#{li.PONumber} and item '{li.BrandName} {li.ModelNumber} {li.PartName}'.";
                    alert.AlertTitle = "Whoops!";
                    alert.AlertType = "alert-danger";
                }
                else
                {
                    var notes = $"Date Received field was cleared for PO#{li.PONumber} and item '{li.BrandName} {li.ModelNumber} {li.PartName}' by {_user} on {DateTime.Now}.";
                    if (nhlcInv.Qty - li.Qty > 0)
                        _repo.EditInventory(nhlcInv.InventoryId, nhlcInv.Qty - li.Qty, nhlcInv.RecycledQty, notes, "System");
                    else
                    {
                        _repo.EditInventory(nhlcInv.InventoryId, nhlcInv.Qty - li.Qty, nhlcInv.RecycledQty, notes, "System");
                        _repo.DeleteInventory(nhlcInv.InventoryId);
                    }
                    _repo.EditPurchaseOrderLI(new PurchaseOrderLI { POLineItemId = model.POLineItemId, Qty = li.Qty, TotalPrice = li.TotalPrice, DateReceived = model.DateReceived });
                    alert.AlertMessage = $"Clearing the Date Received field for PO#{li.PONumber} and item '{li.BrandName} {li.ModelNumber} {li.PartName}' has also negated the inventory gain for this item at NHLC. Inventory at NHLC for this item has been lowered by {li.Qty}.";
                    alert.AlertTitle = "Attention!";
                    alert.AlertType = "alert-warning";
                }
            }
            else if(!li.DateReceived.HasValue && model.DateReceived.HasValue)
            {
                _repo.AddInventory(new Inventory { ModelPartId = li.ModelPartId, SchoolId = nhlcSchId, Qty = model.Qty, LastModifiedBy = "System" }, false);
                _repo.EditPurchaseOrderLI(new PurchaseOrderLI { POLineItemId = model.POLineItemId, Qty = model.Qty, TotalPrice = model.TotalPrice, DateReceived = model.DateReceived });
                alert.AlertMessage = $"You've confirmed that {model.Qty} of {li.BrandName} {li.ModelNumber} {li.PartName} have been received on {model.DateReceived.Value.ToShortDateString()}. These items have been added to NHLC's inventory.";
                alert.AlertTitle = "Success!";
                alert.AlertType = "alert-success";
            }
            else
            {
                if (model.Qty == 0 && !model.DateReceived.HasValue) model.Qty = li.Qty;
                if (li.DateReceived.HasValue) model.DateReceived = li.DateReceived;
                _repo.EditPurchaseOrderLI(new PurchaseOrderLI { POLineItemId = model.POLineItemId, Qty = model.Qty, TotalPrice = model.TotalPrice, DateReceived = model.DateReceived });
                alert.AlertMessage = $"Changes to PO#{li.PONumber} for {li.BrandName} {li.ModelNumber} {li.PartName} have been saved.";
                alert.AlertTitle = "Success!";
                alert.AlertType = "alert-success";
            }
            _alert = alert;

            return RedirectToAction("ViewLineItems", new { po = li.PONumber });
        }

        public ActionResult DeleteLineItem(int lineItemId)
        {
            var li = _repo.GetPurchaseOrderLI(lineItemId);
            var deleted = _repo.DeletePurchaseOrderLI(lineItemId);
            var lineItems = _repo.GetPurchaseOrderLIs(li.PONumber);
            if(lineItems.Count == 0)
            {
                _repo.DeletePurchaseOrder(li.PONumber);
                return RedirectToAction("PurchaseOrders");
            }

            return RedirectToAction("ViewLineItems", new { po = li.PONumber });
        }

        public ActionResult InventoryLog()
        {
            var transfers = _repo.GetInventoryTransfers();
            var invHistory = _repo.GetInvEditHistory();
            var model = new InventoryLogVM { InvTransfers = transfers, InvEditHistory = invHistory };
            return View(model);
        }

        public ActionResult EditNotes(int InvEditId, string Notes)
        {
            _repo.EditInvEditNotes(InvEditId, string.IsNullOrWhiteSpace(Notes) ? null : Notes);
            return RedirectToAction("InventoryLog");
        }

        public ActionResult AcceptInventoryRequest(int id)
        {
            var notification = _repo.GetNotifications(_user).First(n => n.NotificationId == id);
            var recycled = notification.NotifType == "InvRequestRec" ? true : false;
            var nhlcInv = _repo.GetInventory(notification.ModelPartId.Value, 7);
            _repo.TransferInventory(nhlcInv.InventoryId, notification.SchoolId.Value, notification.Qty.Value, _user, recycled);
            _repo.AddNotification(notification.FromUsername, $"{notification.Username} accepted your request and transferred {notification.Qty} {nhlcInv.BrandName} {nhlcInv.ModelNumber} {nhlcInv.PartName}s to your school's inventory.", "Accepted", _user);
            _repo.EditNotification(id, $"(Fulfilled) {notification.NotifMessage}", "InvReqFulfilled");
            return RedirectToAction("Inventory", "Home");
        }

        public ActionResult DeclineInventoryRequest(int id)
        {
            var notification = _repo.GetNotifications(_user).First(n => n.NotificationId == id);
            var mp = _repo.GetModelParts().First(m => m.ModelPartId == notification.ModelPartId);
            var partName = $"{mp.BrandName} {mp.ModelNumber} {mp.PartName}";
            notification.NotifMessage = $"(Declined) {notification.NotifMessage}";
            _repo.AddNotification(notification.FromUsername, $"{notification.Username} has declined your inventory request of {notification.Qty} {partName}s.", "Declined", _user);
            _repo.EditNotification(notification.NotificationId, notification.NotifMessage, "Declined");
            return RedirectToAction("Notifications", "Home");

        }

        public ActionResult LoadMPCostHistoryTable(int mpId)
        {
            return PartialView("CostHistory", _repo.GetPartCostHistory(mpId));
        }
    }
}