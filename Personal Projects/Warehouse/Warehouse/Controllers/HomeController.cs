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
                order.DateOrdered = DateTime.Now;
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
        public ActionResult EditOrderLine(int orderId = 0, int prodId = 0)
        {
            return View(wr.GetOrderLine(orderId, prodId));
        }

        [HttpPost]
        public ActionResult EditOrderLine(OrderLine ol)
        {
            if (ModelState.IsValid)
            {
                wr.EditOrderLine(ol);
                View("GetOrders", wr.GetOrders());
            }

            return View(ol);
        }

        [HttpGet]
        public ActionResult GetOrderLine(int orderId = 0, int prodId = 0)
        {
            return View(wr.GetOrderLine(orderId, prodId));
        }

        [HttpGet]
        public ActionResult DeleteOrderLine(int orderId = 0, int prodId = 0)
        {
            return View(wr.GetOrderLine(orderId, prodId));
        }

        [HttpPost]
        public ActionResult DeleteOrderLine(OrderLine ol)
        {
            wr.DeleteOrderLine(ol.OrderLineId);
            return View("Index");
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
            invVM.SetListItems("create");
            return View(invVM);
        }

        [HttpPost]
        public ActionResult CreateInventory(InventoryVM inv)
        {
            if (ModelState.IsValid)
            {
                var spaceTaken = inv.GetProductInfo().Size * inv.Qty;
                var spaceDiff = inv.GetBinInfo().AvailableSpace - spaceTaken;
                if (spaceDiff < 0)
                {
                    ModelState.AddModelError("", $"The inventory to be added exceeds the bin space by {spaceDiff * -1}.");
                    inv.SetListItems("create");
                    return View(inv);
                }

                var getInv = wr.GetInventory(0, inv.ProductId, inv.BinId);
                if (getInv.Count == 0)
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
                    var newInv = getInv.FirstOrDefault(i => i.BinId == inv.BinId);
                    if (newInv == null || newInv.BinId != inv.BinId)
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
                        newInv.Qty += inv.Qty;
                        wr.EditInventory(newInv);
                    }
                }

                var bin = wr.GetBin(inv.BinId, "");
                bin.AvailableSpace -= spaceTaken;
                wr.EditBin(bin);

                return View("GetAllInventory", wr.GetAllInventory());
            }

            inv.SetListItems("create");
            return View(inv);
        }

        [HttpGet]
        public ActionResult EditInventory(int invId = 0, int prodId = 0, int binId = 0)
        {
            var inv = wr.GetInventory(invId, prodId, binId)[0];
            var invVM = new InventoryVM
            {
                InventoryId = inv.InventoryId,
                OldBinId = inv.BinId,
                BinId = inv.BinId,
                ProductId = inv.ProductId,
                Qty = inv.Qty,
                OldQty = inv.Qty
            };
            invVM.SetListItems("edit");
            return View(invVM);
        }

        [HttpPost]
        public ActionResult EditInventory(InventoryVM inv)
        {
            if (ModelState.IsValid)
            {
                var spaceTaken = inv.GetProductInfo().Size * Math.Abs(inv.Qty - inv.OldQty);
                var binInfo = inv.GetBinInfo();
                bool addedQty = inv.Qty >= inv.OldQty;
                var spaceDiff = addedQty ? binInfo.AvailableSpace - spaceTaken : binInfo.AvailableSpace + spaceTaken;
                if ((spaceDiff < 0 && addedQty) || (spaceDiff > binInfo.Capacity - binInfo.AvailableSpace && !addedQty))
                {
                    ModelState.AddModelError("", $"The inventory to be added exceeds the bin space by {spaceDiff * -1}.");
                    inv.SetListItems("edit");
                    return View(inv);
                }
                
                if (inv.OldBinId == inv.BinId)
                {
                    wr.EditInventory(new Inventory
                    {
                        InventoryId = inv.InventoryId,
                        ProductId = inv.ProductId,
                        BinId = inv.BinId,
                        Qty = addedQty ? inv.OldQty + inv.Qty : inv.OldQty - inv.Qty
                    });
                    binInfo.AvailableSpace = addedQty ? binInfo.AvailableSpace -= spaceTaken : binInfo.AvailableSpace += spaceTaken;
                    wr.EditBin(binInfo);
                }
                else
                {
                    wr.EditInventory(new Inventory
                    {
                        InventoryId = inv.InventoryId,
                        ProductId = inv.ProductId,
                        BinId = inv.OldBinId,
                        Qty = inv.OldQty - Math.Abs(inv.Qty - inv.OldQty)
                    });

                    var oldBin = inv.GetOldBinInfo();
                    oldBin.AvailableSpace += inv.GetProductInfo().Size * inv.Qty;
                    wr.EditBin(oldBin);

                    var getInv = wr.GetInventory(0, inv.ProductId, inv.BinId);
                    Inventory newInv;
                    if(getInv.Count == 0)
                    {
                        wr.AddInventory(new Inventory
                        {
                            ProductId = inv.ProductId,
                            BinId = inv.BinId,
                            Qty = inv.Qty
                        });
                    }
                    else
                    {
                        newInv = getInv.FirstOrDefault(i => i.BinId == inv.BinId);
                        newInv.Qty += inv.Qty;
                        wr.EditInventory(newInv);
                    }
                    binInfo.AvailableSpace -= spaceTaken;
                    wr.EditBin(binInfo);
                }
                return View("GetAllInventory", wr.GetAllInventory());
            }

            return View(inv);
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
            wr.DeleteInventory(inv.InventoryId, inv.ProductId, inv.BinId);
            return View("GetAllInventory", wr.GetAllInventory());
        }
    }
}