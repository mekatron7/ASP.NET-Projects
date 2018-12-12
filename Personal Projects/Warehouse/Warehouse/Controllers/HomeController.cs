using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data2;
using Warehouse.Models;

namespace Warehouse.Controllers
{
    public class HomeController : Controller
    {
        private WarehouseRepo wr = new WarehouseRepo();

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetProducts()
        {
            return View(wr.GetProducts());
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            return View(new Product());
        }

        [HttpPost]
        public ActionResult CreateProduct(Product prod)
        {
            if (ModelState.IsValid)
            {
                if(prod.Size <= 0)
                {
                    ModelState.AddModelError("", "You can't have a zero size. That makes no damn sense.");
                    return View(prod);
                }
                if(wr.GetProducts().Select(p => p.SKU).Contains(prod.SKU))
                {
                    ModelState.AddModelError("", $"A product with the SKU '{prod.SKU}' already exists.");
                    return View(prod);
                }
                wr.AddProduct(prod);
                return View("GetProducts", wr.GetProducts());
            }

            return View(prod);
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            return View(wr.GetProduct(id));
        }

        [HttpPost]
        public ActionResult EditProduct(Product prod)
        {
            if (ModelState.IsValid)
            {
                wr.EditProduct(prod);
                return View("GetProducts", wr.GetProducts());
            }

            return View(prod);
        }

        [HttpGet]
        public ActionResult GetProduct(int id)
        {
            return View(wr.GetProduct(id));
        }

        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            return View(wr.GetProduct(id));
        }

        [HttpPost]
        public ActionResult DeleteProduct(Product prod)
        {
            if(wr.GetInventory(0, prod.ProductId, 0).Count > 0)
            {
                ModelState.AddModelError("", "This product can't be deleted because it still has existing inventory.");
                return View(prod);
            }
            wr.DeleteProduct(prod.ProductId);
            return View("GetProducts", wr.GetProducts());
        }

        [HttpGet]
        public ActionResult GetOrders()
        {
            return View(wr.GetOrders());
        }

        [HttpGet]
        public ActionResult CreateOrder()
        {
            return View(new Order());
        }

        [HttpPost]
        public ActionResult CreateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                order.DateOrdered = DateTime.Now;
                wr.AddOrder(order);
                return View("GetOrders", wr.GetOrders());
            }

            return View(order);
        }

        [HttpGet]
        public ActionResult EditOrder(int orderNum)
        {
            return View(wr.GetOrder(orderNum));
        }

        [HttpPost]
        public ActionResult EditOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                wr.EditOrder(order);
                return View("GetOrders", wr.GetOrders());
            }

            return View(order);
        }

        [HttpGet]
        public ActionResult GetOrder(int orderNum)
        {
            return View(wr.GetOrder(orderNum));
        }

        [HttpGet]
        public ActionResult DeleteOrder(int orderNum)
        {
            return View(wr.GetOrder(orderNum));
        }

        [HttpPost]
        public ActionResult DeleteOrder(Order order)
        {
            order = wr.GetOrder(order.OrderNumber);
            var orderLines = wr.GetOrderLines(order.OrderNumber);
            var totalAvailSpace = wr.GetBins().Sum(b => b.AvailableSpace);
            var totalSpaceTaken = 0;
            foreach(var ol in orderLines)
            {
                totalSpaceTaken += ol.Qty * ol.GetProductInfo().Size;
            }
            if(totalSpaceTaken > totalAvailSpace)
            {
                ModelState.AddModelError("", "There's not enough bin space to delete the order. Try deleting individual order lines first.");
                return View(order);
            }
            foreach(var ol in orderLines)
            {
                DeleteOrderLineBinInv(ol);
            }

            wr.DeleteOrder(order.OrderNumber);
            return View("GetOrders", wr.GetOrders());
        }

        [HttpGet]
        public ActionResult GetBins()
        {
            return View(wr.GetBins());
        }

        [HttpGet]
        public ActionResult CreateBin()
        {
            return View(new Bin());
        }

        [HttpPost]
        public ActionResult CreateBin(Bin bin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    wr.AddBin(bin);
                }
                catch(Exception ex)
                {

                }
                return View("GetBins", wr.GetBins());
            }

            return View(bin);
        }

        [HttpGet]
        public ActionResult EditBin(int id = 0, string name = "")
        {
            return View(wr.GetBin(id, name));
        }

        [HttpPost]
        public ActionResult EditBin(Bin bin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    wr.EditBin(bin);
                }
                catch (Exception ex)
                {

                }
                return View("GetBins", wr.GetBins());
            }

            return View(bin);
        }

        [HttpGet]
        public ActionResult BinContents(int id)
        {
            return View(new BinContents { Contents = wr.GetInventory(0, 0, id), BinName = wr.GetBin(id, "").BinName });
        }

        [HttpGet]
        public ActionResult GetBin(int id = 0, string name = "")
        {
            return View(wr.GetBin(id, name));
        }

        [HttpGet]
        public ActionResult DeleteBin(string name)
        {
            return View(wr.GetBin(0, name));
        }

        [HttpPost]
        public ActionResult DeleteBin(Bin bin)
        {
            if(bin.AvailableSpace != bin.Capacity)
            {
                ModelState.AddModelError("", "This bin cannot be deleted until all inventory has been removed.");
                return View(bin);
            }
            wr.DeleteBin(bin);
            return View("GetBins", wr.GetBins());
        }

        [HttpGet]
        public ActionResult CreateOrderLine()
        {
            return View(new Order());
        }

        [HttpPost]
        public ActionResult CreateOrderLine(OrderLine ol)
        {
            if (ModelState.IsValid)
            {
                wr.AddOrderLine(ol);
                View("GetOrders", wr.GetOrders());
            }

            return View(ol);
        }

        [HttpGet]
        public ActionResult ChangeQty(int id)
        {
            var ol = wr.GetOrderLine(id);
            ol.OldQty = ol.Qty;
            return View(ol);
        }

        [HttpPost]
        public ActionResult ChangeQty(OrderLine ol)
        {
            if (ModelState.IsValid)
            {
                var qtyDiff = Math.Abs(ol.Qty - ol.OldQty);
                bool addedQty = ol.Qty >= ol.OldQty;
                if (ol.Qty <= 0)
                {
                    ModelState.AddModelError("", "The quantity must be greater than 0.");
                    ol.Qty = ol.OldQty;
                    return View(ol);
                }
                if(qtyDiff > ol.GetProductInfo().GetInvQuantity() && addedQty)
                {
                    ModelState.AddModelError("", "The order quantity can't exceed the quantity available in inventory.");
                    ol.Qty = ol.OldQty;
                    return View(ol);
                }
                
                Bin bin;
                
                if (addedQty)
                {
                    var invList = wr.GetInventory(0, ol.ProductId, 0).OrderBy(i => i.GetBinInfo().AvailableSpace).ToList();
                    int bininvPosition = 0;

                    do
                    {
                        var currentBinInventory = invList[bininvPosition]; //Current inventory in list
                        int amountToTake = qtyDiff > currentBinInventory.Qty ? currentBinInventory.Qty : qtyDiff; //Amount to take from bin
                        currentBinInventory.Qty -= amountToTake; //Removes desired quantity
                        qtyDiff -= amountToTake; //Subtracts amount taken from total quantity to be retrieved

                        if (currentBinInventory.Qty == 0) //If inv qty becomes 0, delete inv. If not, edit inv.
                        {
                            wr.DeleteInventory(currentBinInventory.InventoryId, 0, 0);
                        }
                        else
                        {
                            wr.EditInventory(currentBinInventory);
                        }
                        bin = currentBinInventory.GetBinInfo(); //Gets the current bin being accessed
                        bin.AvailableSpace += amountToTake * currentBinInventory.GetProductInfo().Size; //Sets the available space freed up in the bin
                        wr.EditBin(bin); //Edits current bin
                        bininvPosition++; //Moves to next bin
                    }
                    while (qtyDiff != 0);
                }
                else
                {
                    var spaceTaken = ol.GetProductInfo().Size * qtyDiff;
                    var bins = wr.GetBins();
                    bin = bins.FirstOrDefault(b => b.AvailableSpace == bins.Max(i => i.AvailableSpace));
                    bin.AvailableSpace -= spaceTaken;
                    var inv = wr.GetInventory(0, 0, bin.BinId).FirstOrDefault(i => i.ProductId == ol.ProductId);
                    if(inv == null)
                    {
                        wr.AddInventory(new Inventory { BinId = bin.BinId, ProductId = ol.ProductId, Qty = qtyDiff });
                    }
                    else
                    {
                        inv.Qty += qtyDiff;
                        wr.EditInventory(inv);
                    }
                    wr.EditBin(bin);
                }
                wr.EditOrderLine(ol);
                return View("ViewOrder", ol.GetOrderInfo());
            }

            return View(ol);
        }

        [HttpGet]
        public ActionResult GetOrderLine(int id)
        {
            return View(wr.GetOrderLine(id));
        }

        [HttpGet]
        public ActionResult DeleteOrderLine(int id)
        {
            return View(wr.GetOrderLine(id));
        }

        [HttpPost]
        public ActionResult DeleteOrderLine(OrderLine ol)
        {
            var order = wr.GetOrder(ol.OrderId);
            if(!DeleteOrderLineBinInv(ol))
            {
                ModelState.AddModelError("", "This order line can't be deleted because there's no space available in the bins.\nCreate a new bin with sufficient space.");
                return View(ol);
            }
            if (wr.GetOrderLines(ol.OrderId).Count == 0) return RedirectToAction("GetOrders");
            return View("ViewOrder", order);
        }

        [HttpGet]
        public ActionResult GetAllInventory()
        {
            return View(wr.GetAllInventory());
        }

        [HttpGet]
        public ActionResult CreateInventory()
        {
            var invVM = new InventoryVM();
            invVM.SetListItems();
            return View(invVM);
        }

        [HttpPost]
        public ActionResult CreateInventory(InventoryVM inv)
        {
            if (ModelState.IsValid)
            {
                var spaceTaken = inv.GetProductInfo().Size * inv.Qty;
                var spaceDiff = inv.GetBinInfo().AvailableSpace - spaceTaken;
                if(inv.Qty <= 0)
                {
                    ModelState.AddModelError("", "The quantity must be greater than 0.");
                    inv.SetListItems();
                    return View(inv);
                }
                if (spaceDiff < 0)
                {
                    ModelState.AddModelError("", $"The inventory to be added exceeds the bin space by {spaceDiff * -1}.");
                    inv.SetListItems();
                    return View(inv);
                }

                var getInv = wr.GetInventory(0, inv.ProductId, inv.BinId).FirstOrDefault(i => i.ProductId == inv.ProductId && i.BinId == inv.BinId);
                if (getInv == null)
                {
                    wr.AddInventory(new Inventory
                    {
                        BinId = inv.BinId,
                        ProductId = inv.ProductId,
                        Qty = inv.Qty
                    });
                }
                else
                {
                    getInv.Qty += inv.Qty;
                    wr.EditInventory(getInv);
                }

                var bin = wr.GetBin(inv.BinId, "");
                bin.AvailableSpace -= spaceTaken;
                wr.EditBin(bin);

                return View("GetAllInventory", wr.GetAllInventory());
            }

            inv.SetListItems();
            return View(inv);
        }

        [HttpGet]
        public ActionResult EditInventory(int invId = 0, int prodId = 0, int binId = 0)
        {
            var inv = wr.GetInventory(invId, prodId, binId)[0];
            var invVM = new InventoryVM
            {
                InventoryId = inv.InventoryId,
                BinId = inv.BinId,
                ProductId = inv.ProductId,
                Qty = inv.Qty,
                OldQty = inv.Qty
            };
            return View(invVM);
        }

        [HttpPost]
        public ActionResult EditInventory(InventoryVM inv)
        {
            if (ModelState.IsValid)
            {
                if (inv.Qty < 0)
                {
                    ModelState.AddModelError("", "The quantity can't be negative.");
                    return View(inv);
                }
                var spaceTaken = inv.GetProductInfo().Size * Math.Abs(inv.Qty - inv.OldQty);
                var binInfo = inv.GetBinInfo();
                bool addedQty = inv.Qty >= inv.OldQty;
                var spaceDiff = addedQty ? binInfo.AvailableSpace - spaceTaken : binInfo.AvailableSpace + spaceTaken;
                if (spaceDiff < 0)
                {
                    ModelState.AddModelError("", $"The inventory to be added exceeds the bin space by {spaceDiff * -1}.");
                    return View(inv);
                }

                wr.EditInventory(new Inventory
                {
                    InventoryId = inv.InventoryId,
                    ProductId = inv.ProductId,
                    BinId = inv.BinId,
                    Qty = inv.Qty
                });
                binInfo.AvailableSpace = addedQty ? binInfo.AvailableSpace -= spaceTaken : spaceDiff;
                wr.EditBin(binInfo);
                if (wr.GetInventory(inv.InventoryId, 0, 0)[0].Qty == 0) wr.DeleteInventory(inv.InventoryId, 0, 0);

                return View("GetAllInventory", wr.GetAllInventory());
            }

            return View(inv);
        }

        [HttpGet]
        public ActionResult TransferInventory(int invId)
        {
            var inv = wr.GetInventory(invId, 0, 0)[0];
            var invVM = new InventoryVM
            {
                InventoryId = inv.InventoryId,
                OldBinId = inv.BinId,
                ProductId = inv.ProductId,
                OldQty = inv.Qty
            };
            invVM.SetTransferListItems();
            return View(invVM);
        }

        [HttpPost]
        public ActionResult TransferInventory(InventoryVM inv)
        {
            if (inv.Qty > inv.OldQty)
            {
                ModelState.AddModelError("", $"You can't transfer more than {inv.OldQty} units.");
                inv.SetTransferListItems();
                return View(inv);
            }
            else if (inv.Qty <= 0)
            {
                ModelState.AddModelError("", "The transfer quantity must be greater than 0.");
                inv.SetTransferListItems();
                return View(inv);
            }
            var spaceTaken = inv.GetProductInfo().Size * inv.Qty;
            var toBinInfo = inv.GetBinInfo();
            var spaceDiff = toBinInfo.AvailableSpace - spaceTaken;
            if ((spaceDiff < 0))
            {
                ModelState.AddModelError("", $"The inventory to be transfered exceeds the destination bin space by {spaceDiff * -1}.");
                inv.SetTransferListItems();
                return View(inv);
            }

            var transferInvInfo = new Inventory
            {
                InventoryId = inv.InventoryId,
                ProductId = inv.ProductId,
                BinId = inv.BinId,
                Qty = inv.Qty
            };
            var toInv = wr.GetInventory(0, inv.ProductId, inv.BinId).Where(i => i.ProductId == inv.ProductId && i.BinId == inv.BinId).ToList();
            if(toInv.Count == 0)
            {
                wr.TransferInventory(transferInvInfo, inv.OldBinId, 0);
            }
            else
            {
                wr.TransferInventory(transferInvInfo, inv.OldBinId, 1);
            }
            var fromBinInfo = inv.GetOldBinInfo();
            fromBinInfo.AvailableSpace += spaceTaken;
            toBinInfo.AvailableSpace -= spaceTaken;
            wr.EditBin(fromBinInfo);
            wr.EditBin(toBinInfo);
            if (wr.GetInventory(inv.InventoryId, 0, 0)[0].Qty == 0) wr.DeleteInventory(inv.InventoryId, 0, 0);

            return View("GetAllInventory", wr.GetAllInventory());
        }

        [HttpGet]
        public ActionResult GetInventory(int invId = 0, int prodId = 0, int binId = 0)
        {
            return View(wr.GetInventory(invId, prodId, binId));
        }

        [HttpGet]
        public ActionResult DeleteInventory(int invId = 0, int prodId = 0, int binId = 0)
        {
            return View(wr.GetInventory(invId, prodId, binId)[0]);
        }

        [HttpPost]
        public ActionResult DeleteInventory(Inventory inv)
        {
            wr.DeleteInventory(inv.InventoryId, 0, 0);
            var binInv = wr.GetBin(inv.BinId, "");
            binInv.AvailableSpace += inv.GetProductInfo().Size * inv.Qty;
            wr.EditBin(binInv);
            return View("GetAllInventory", wr.GetAllInventory());
        }

        [HttpGet]
        public ActionResult AddToOrder(int id)
        {
            var model = new AddToOrderVM { Prod = wr.GetProduct(id), ProdId = id };
            model.SetAddToOrderLists();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddToOrder(AddToOrderVM orderLine)
        {
            var prod = wr.GetProduct(orderLine.ProdId);
            orderLine.Prod = prod;
            if (ModelState.IsValid)
            {
                if(orderLine.Qty > prod.GetInvQuantity())
                {
                    ModelState.AddModelError("", "You can't add more than what available in inventory.");
                    orderLine.SetAddToOrderLists();
                    return View(orderLine);
                }

                //Goes through the inventory and automatically gets inventory from other bins in descending qty order if amount
                //demanded is higher than initial bin qty
                var qtyToGet = orderLine.Qty;
                var binInvList = wr.GetInventory(0, orderLine.Prod.ProductId, 0).Where(i => i.BinId != orderLine.BinId).OrderByDescending(b => b.Qty).ToList();
                binInvList.Insert(0, wr.GetInventory(0, orderLine.Prod.ProductId, 0).First(i => i.BinId == orderLine.BinId));
                int bininvPosition = 0;

                do
                {
                    var currentBinInventory = binInvList[bininvPosition]; //Current inventory in list
                    int amountToTake = qtyToGet > currentBinInventory.Qty ? currentBinInventory.Qty : qtyToGet; //Amount to take from bin
                    currentBinInventory.Qty -= amountToTake; //Removes desired quantity
                    qtyToGet -= amountToTake; //Subtracts amount taken from total quantity to be retrieved
                    
                    if (currentBinInventory.Qty == 0) //If inv qty becomes 0, delete inv. If not, edit inv.
                    {
                        wr.DeleteInventory(currentBinInventory.InventoryId, 0, 0);
                    }
                    else
                    {
                        wr.EditInventory(currentBinInventory);
                    }
                    var bin = currentBinInventory.GetBinInfo(); //Gets the current bin being accessed
                    bin.AvailableSpace += amountToTake * currentBinInventory.GetProductInfo().Size; //Sets the available space freed up in the bin
                    wr.EditBin(bin); //Edits current bin
                    bininvPosition++; //Moves to next bin
                }
                while (qtyToGet != 0); //If qtyToGet hasn't been reached yet, go to next bin and retrieve inventory

                var ol = wr.GetOrderLine(orderLine.OrderNum, orderLine.ProdId);
                if (ol == null)
                {
                    wr.AddOrderLine(new OrderLine { OrderId = orderLine.OrderNum, ProductId = orderLine.Prod.ProductId, Qty = orderLine.Qty });
                }
                else
                {
                    ol.Qty += orderLine.Qty;
                    wr.EditOrderLine(ol);
                }
                
                return RedirectToAction("GetProducts");
            }

            orderLine.SetAddToOrderLists();
            return View(orderLine);
        }

        [HttpGet]
        public ActionResult ViewOrder(int id)
        {
            return View(wr.GetOrder(id));
        }

        public bool DeleteOrderLineBinInv(OrderLine ol)
        {
            bool success = true;
            var binToReturnInv = wr.GetBins().OrderByDescending(b => b.AvailableSpace).ToList()[0];
            var spaceTaken = ol.Qty * ol.GetProductInfo().Size;
            if (binToReturnInv.AvailableSpace < spaceTaken)
            {
                success = false;
                return success;
            }
            var invList = wr.GetInventory(0, ol.ProductId, 0);
            if (invList.Count == 0)
            {
                wr.AddInventory(new Inventory
                {
                    BinId = binToReturnInv.BinId,
                    ProductId = ol.ProductId,
                    Qty = ol.Qty
                });
                binToReturnInv.AvailableSpace -= spaceTaken;
                wr.EditBin(binToReturnInv);
            }
            else
            {
                var binList = new List<Bin>();
                foreach (var inv in invList)
                {
                    binList.Add(inv.GetBinInfo());
                }
                var mostSpaceBin = binList.OrderByDescending(b => b.AvailableSpace).ToList()[0];
                mostSpaceBin.AvailableSpace -= spaceTaken;
                var returnedInv = invList.First(i => i.BinId == mostSpaceBin.BinId);
                returnedInv.Qty += ol.Qty;
                wr.EditInventory(returnedInv);
                wr.EditBin(mostSpaceBin);
            }
            wr.DeleteOrderLine(ol.OrderLineId);

            return success;
        }
    }
}