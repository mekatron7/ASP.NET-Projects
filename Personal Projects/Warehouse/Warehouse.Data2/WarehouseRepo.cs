using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Data2
{
    public class WarehouseRepo
    {
        string connString = ConfigurationManager.ConnectionStrings["Warehouse"].ConnectionString;

        public void AddProduct(Product prod)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SKU", prod.SKU);
                parameters.Add("@ProductDescrip", prod.ProductDescription);
                parameters.Add("@Size", prod.Size);

                cn.Execute("AddProduct", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Product GetProduct(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ProdId", id);

                return cn.QueryFirstOrDefault<Product>("GetProduct", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Product> GetProducts()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<Product>("GetProducts", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void EditProduct(Product prod)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ProdId", prod.ProductId);
                parameters.Add("@SKU", prod.SKU);
                parameters.Add("@ProductDescrip", prod.ProductDescription);

                cn.Execute("EditProduct", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteProduct(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ProdId", id);

                return cn.Execute("DeleteProduct", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public void AddBin(Bin bin)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@BinName", bin.BinName);
                parameters.Add("@Capacity", bin.Capacity);

                cn.Execute("AddBin", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Bin GetBin(int id, string name)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@BinName", name);
                parameters.Add("@BinId", id);

                return cn.QueryFirstOrDefault<Bin>("GetBin", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Bin> GetBins()
        {
            using(var cn = new SqlConnection(connString))
            {
                return cn.Query<Bin>("GetBins", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void EditBin(Bin bin)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@BinId", bin.BinId);
                parameters.Add("@BinName", bin.BinName);
                parameters.Add("@AvailableSpace", bin.AvailableSpace);

                cn.Execute("EditBin", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteBin(Bin bin)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@BinName", bin.BinName);
                parameters.Add("@BinId", bin.BinId);

                return cn.Execute("DeleteBin", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public void AddInventory(Inventory inv)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ProductId", inv.ProductId);
                parameters.Add("@BinId", inv.BinId);
                parameters.Add("@Qty", inv.Qty);

                cn.Execute("AddInventory", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Inventory> GetInventory(int invId, int prodId, int binId)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@InventoryId", invId);
                parameters.Add("@ProductId", prodId);
                parameters.Add("@BinId", binId);

                return cn.Query<Inventory>("GetInventory", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<Inventory> GetAllInventory()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<Inventory>("GetAllInventory", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void EditInventory(Inventory inv)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@InventoryId", inv.InventoryId);
                parameters.Add("@Qty", inv.Qty);

                cn.Execute("EditInventory", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void TransferInventory(Inventory inv, int fromBinId, byte invExists)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ProductId", inv.ProductId);
                parameters.Add("@TransferAmount", inv.Qty);
                parameters.Add("@FromBinId", fromBinId);
                parameters.Add("@ToBinId", inv.BinId);
                parameters.Add("@InvExists", invExists);

                cn.Execute("TransferInventory", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteInventory(int invId, int prodId, int binId)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@InventoryId", invId);
                parameters.Add("@ProductId", prodId);
                parameters.Add("@BinId", binId);

                return cn.Execute("DeleteInventory", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public void AddOrder(Order order)
        {
            using(var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@DateOrdered", order.DateOrdered);
                parameters.Add("@CustomerName", order.CustomerName);
                parameters.Add("@CustomerAddress", order.CustomerAddress);

                cn.Execute("AddOrder", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Order GetOrder(int orderNum)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@OrderNum", orderNum);

                return cn.QueryFirstOrDefault<Order>("GetOrder", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Order> GetOrders()
        {
            using(var cn = new SqlConnection(connString))
            {
                return cn.Query<Order>("GetOrders", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void EditOrder(Order order)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@OrderNum", order.OrderNumber);
                parameters.Add("@DateOrdered", order.DateOrdered);
                parameters.Add("@CustomerName", order.CustomerName);
                parameters.Add("@CustomerAddress", order.CustomerAddress);

                cn.Execute("EditOrder", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteOrder(int orderNum)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@OrderNum", orderNum);

                return cn.Execute("DeleteOrder", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public void AddOrderLine(OrderLine ol)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@OrderId", ol.OrderId);
                parameters.Add("@ProductId", ol.ProductId);
                parameters.Add("@Qty", ol.Qty);

                cn.Execute("AddOrderLine", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public OrderLine GetOrderLine(int orderId, int prodId)
        {
            using(var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@OrderId", orderId);
                parameters.Add("@ProductId", prodId);

                return cn.QueryFirstOrDefault<OrderLine>("GetOrderLine", parameters, commandType: CommandType.StoredProcedure);

            }
        }

        public List<OrderLine> GetOrderLines(int orderNum)
        {
            using(var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@OrderId", orderNum);

                return cn.Query<OrderLine>("GetOrderLines", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void EditOrderLine(OrderLine ol)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@OrderLineId", ol.OrderLineId);
                parameters.Add("@Qty", ol.Qty);

                cn.Execute("EditOrderLine", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteOrderLine(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@OrderLineId", id);

                return cn.Execute("DeleteOrderLine", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }
    }
}
