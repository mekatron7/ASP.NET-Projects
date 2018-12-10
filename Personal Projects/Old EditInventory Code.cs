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