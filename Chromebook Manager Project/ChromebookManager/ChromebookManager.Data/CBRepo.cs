using ChromebookManager.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Data
{
    public class CBRepo
    {
        string connString = ConfigurationManager.ConnectionStrings["ChromebookDB"].ConnectionString;
        string connString2 = ConfigurationManager.ConnectionStrings["DestinyDB"].ConnectionString;

        public void AddBrand(Brand brand)
        {
            using(var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@BrandName", brand.BrandName);
                parameters.Add("@AddedBy", brand.AddedBy);

                cn.Execute("AddBrand", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteBrand(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@BrandId", id);

                return cn.Execute("DeleteBrand", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Brand> GetBrands()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<Brand>("GetBrands", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void AddModel(Model model)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ModelNumber", model.ModelNumber);
                parameters.Add("@BrandId", model.BrandId);
                parameters.Add("@AddedBy", model.AddedBy);
                parameters.Add("@Cost", model.Cost);

                cn.Execute("AddModel", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditModelCost(int modelId, decimal cost, string modifiedBy)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ModelId", modelId);
                parameters.Add("@Cost", cost);
                parameters.Add("@ModifiedBy", modifiedBy);

                cn.Execute("EditModelCost", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteModel(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ModelId", id);

                return cn.Execute("DeleteModel", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Model> GetModels()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<Model>("GetModels", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void AddPart(Part part)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@PartName", part.PartName);
                parameters.Add("@AddedBy", part.AddedBy);

                cn.Execute("AddPart", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeletePart(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@PartId", id);

                return cn.Execute("DeletePart", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Part> GetParts()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<Part>("GetParts", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void AddModelPart(ModelPart mp)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ModelId", mp.ModelId);
                parameters.Add("@PartId", mp.PartId);
                parameters.Add("@Cost", mp.Cost);
                parameters.Add("@AddedBy", mp.AddedBy);

                cn.Execute("AddModelPart", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditModelPart(int id, decimal cost, string modifiedBy)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ModelPartId", id);
                parameters.Add("@Cost", cost);
                parameters.Add("@ModifiedBy", modifiedBy);

                cn.Execute("EditModelPart", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteModelPart(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ModelPartId", id);

                return cn.Execute("DeleteModelPart", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<ModelPart> GetModelParts()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<ModelPart>("GetModelParts", commandType: CommandType.StoredProcedure).OrderBy(mp => mp.ModelPartId).ToList();
            }
        }

        public Client AddClient(Client client)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ClientId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Username", client.Username);
                if (client.Grade > 0) parameters.Add("@Grade", client.Grade);
                else parameters.Add("@Grade", null);
                parameters.Add("@AddedBy", client.AddedBy);
                parameters.Add("@DoesNotExist", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                cn.Execute("AddClient", parameters, commandType: CommandType.StoredProcedure);

                client.DoesNotExist = parameters.Get<bool>("@DoesNotExist");
                if(!client.DoesNotExist) client.ClientId = parameters.Get<int>("@ClientId");

                return client;
            }
        }

        public Client AddClientAutomatic(Client client)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ClientId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Username", client.Username);
                parameters.Add("@AddedBy", client.AddedBy);
                parameters.Add("@DoesNotExist", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@NotEnrolled", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@SchoolId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                cn.Execute("AddClientAutomatic", parameters, commandType: CommandType.StoredProcedure);

                client.DoesNotExist = parameters.Get<bool>("@DoesNotExist");
                if (!client.DoesNotExist)
                {
                    client.NotEnrolled = parameters.Get<bool>("@NotEnrolled");
                    if (!client.NotEnrolled)
                    {
                        client.ClientId = parameters.Get<int>("@ClientId");
                        client.SchoolId = parameters.Get<int>("@SchoolId");
                    }
                }

                return client;
            }
        }

        public int AddUnassignedClient(string addedBy)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@AddedBy", addedBy);
                parameters.Add("@ClientId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                cn.Execute("AddUnassignedClient", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@ClientId");
            }
        }

        public void EditClient(int id, int grade)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ClientId", id);
                parameters.Add("@Grade", grade);

                cn.Execute("EditClient", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void NextGrade()
        {
            using (var cn = new SqlConnection(connString))
            {
                cn.Execute("NextGrade", commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteClient(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ClientId", id);

                return cn.Execute("DeleteClient", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Client> GetAllClients()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<Client>("GetAllClients", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<Client> GetClientsBySchool(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SchoolId", id);

                return cn.Query<Client>("GetClientsBySchool", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Client GetClient(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ClientId", id);

                return cn.QueryFirstOrDefault<Client>("GetClient", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Client GetClientByUsername(string username)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);

                return cn.QueryFirstOrDefault<Client>("GetClientByUsername", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Client GetUnassignedClient()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.QueryFirstOrDefault<Client>("GetUnassignedClient", commandType: CommandType.StoredProcedure);
            }
        }

        public int AddDevice(Device device, int? clientId = null, int? schoolId = null)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Barcode", device.Barcode);
                parameters.Add("@SerialNumber", device.SerialNumber);
                parameters.Add("@StorageCapacity", device.StorageCapacity);
                parameters.Add("@ModelId", device.ModelId);
                parameters.Add("@AddedBy", device.AddedBy);
                if (schoolId.HasValue) parameters.Add("@SchoolId", schoolId.Value);
                else parameters.Add("@SchoolId", DBNull.Value);
                if (clientId.HasValue) parameters.Add("@ClientId", clientId.Value);
                else parameters.Add("@ClientId", DBNull.Value);
                parameters.Add("@ClientDeviceId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                cn.Execute("AddDevice", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@ClientDeviceId");
            }
        }

        public int AddClientDevice(int clientId, int schoolId, string barcode, string addedBy)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ClientId", clientId);
                parameters.Add("@SchoolId", schoolId);
                parameters.Add("@Barcode", barcode);
                parameters.Add("@AddedBy", addedBy);
                parameters.Add("@ClientDeviceId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                cn.Execute("AddClientDevice", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@ClientDeviceId");
            }
        }

        public void EditDevice(Device device)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Barcode", device.Barcode);
                parameters.Add("@SerialNumber", device.SerialNumber);
                parameters.Add("@StorageCapacity", device.StorageCapacity);

                cn.Execute("EditDevice", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteDevice(string barcode)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Barcode", barcode);

                return cn.Execute("DeleteDevice", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public Device GetDevice(string barcode)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Barcode", barcode);

                return cn.QueryFirstOrDefault<Device>("GetDevice", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Device> GetAllDevices()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<Device>("GetAllDevices", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<Device> GetDevicesBySchool(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SchoolId", id);

                return cn.Query<Device>("GetDevicesBySchool", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<Device> GetDevicesByClient(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ClientId", id);

                return cn.Query<Device>("GetDevicesByClient", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<School> GetSchools()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<School>("GetSchools", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void AddIssueType(IssueType issueType)
        {
            using( var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IssueName", issueType.IssueName);
                parameters.Add("@IssueDescription", issueType.IssueDescription);
                parameters.Add("@AddedBy", issueType.AddedBy);

                cn.Execute("AddIssueType", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditIssueType(IssueType issueType)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IssueId", issueType.IssueId);
                parameters.Add("@IssueName", issueType.IssueName);
                parameters.Add("@IssueDescription", issueType.IssueDescription);

                cn.Execute("EditIssueType", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteIssueType(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@IssueId", id);

                return cn.Execute("DeleteIssueType", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<IssueType> GetIssueTypes()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<IssueType>("GetIssueTypes", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void AddRepairLog(RepairLog log)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ClientDeviceId", log.ClientDeviceId);
                parameters.Add("@IssueId", log.IssueId);
                parameters.Add("@IssueDescription", log.IssueDescription);
                parameters.Add("@EmailAddress", log.EmailAddress);
                parameters.Add("@AddedBy", log.AddedBy);
                parameters.Add("@RepairId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                cn.Execute("AddRepairLog", parameters, commandType: CommandType.StoredProcedure);

                log.RepairId = parameters.Get<int>("@RepairId");
            }
        }

        public void AddPartUsed(int repairId, int inventoryId, bool recycled)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@InventoryId", inventoryId);
                parameters.Add("@RepairId", repairId);
                parameters.Add("@Recycled", recycled);

                cn.Execute("AddPartUsed", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void RemovePartUsed(int partUsedId)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PartUsedId", partUsedId);

                cn.Execute("RemovePartUsed", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<PartUsed> GetPartsUsed(int repairId)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RepairId", repairId);

                return cn.Query<PartUsed>("GetPartsUsed", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<PartUsed> GetPartsUsedByClient(int clientId)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ClientId", clientId);

                return cn.Query<PartUsed>("GetPartsUsedByClient", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void EditRepairLog(RepairLog log)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RepairId", log.RepairId);
                parameters.Add("@RepairNotes", log.RepairNotes);
                if(log.RepairReturnedDate.HasValue)
                    parameters.Add("@RepairReturnedDate", log.RepairReturnedDate.Value);
                else parameters.Add("@RepairReturnedDate", null);
                if(log.WarrantyRepairSentDate.HasValue)
                    parameters.Add("@WarrantyRepairSentDate", log.WarrantyRepairSentDate.Value);
                else parameters.Add("@WarrantyRepairSentDate", null);
                parameters.Add("@Notes", log.Notes);

                cn.Execute("EditRepairLog", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteRepairLog(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@RepairId", id);

                int deleteCount = cn.Execute("DeleteRepairLog", parameters, commandType: CommandType.StoredProcedure);
                return deleteCount > 0;
            }
        }

        public List<RepairLog> GetRepairLogs()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<RepairLog>("GetRepairLogs", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<RepairLog> GetRepairLogBySchool(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SchoolId", id);

                return cn.Query<RepairLog>("GetRepairLogBySchool", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<RepairLog> GetRepairLogByClient(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ClientId", id);

                return cn.Query<RepairLog>("GetRepairLogByClient", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<RepairLog> GetRepairLogByDevice(string barcode)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Barcode", barcode);

                return cn.Query<RepairLog>("GetRepairLogByDevice", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<RepairLog> GetOpenRepairLogs()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<RepairLog>("GetOpenRepairLogs", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<RepairLog> GetFulfilledRepairLogs()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<RepairLog>("GetFulfilledRepairLogs", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public RepairLog GetRepairLog(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RepairId", id);

                return cn.QueryFirstOrDefault<RepairLog>("GetRepairLog", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddInventory(Inventory inv, bool recycled)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ModelPartId", inv.ModelPartId);
                parameters.Add("@SchoolId", inv.SchoolId);
                parameters.Add("@Qty", inv.Qty);
                parameters.Add("@LastModifiedBy", inv.LastModifiedBy);
                parameters.Add("@Recycled", recycled);

                cn.Execute("AddInventory", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditInventory(int id, int qty, int recycledQty, string notes, string modifiedBy)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@InventoryId", id);
                parameters.Add("@Qty", qty);
                parameters.Add("@RecycledQty", recycledQty);
                parameters.Add("@Notes", notes);
                parameters.Add("@LastModifiedBy", modifiedBy);

                cn.Execute("EditInventory", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteInventory(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@InventoryId", id);

                return cn.Execute("DeleteInventory", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Inventory> GetAllInventory()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<Inventory>("GetAllInventory", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Inventory GetInventory(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@InventoryId", id);

                return cn.QueryFirstOrDefault<Inventory>("GetInventory", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Inventory GetInventory(int modelPartId, int schoolId)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ModelPartId", modelPartId);
                parameters.Add("@SchoolId", schoolId);

                return cn.QueryFirstOrDefault<Inventory>("GetInventory2", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Inventory> GetInventoryBySchool(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SchoolId", id);

                return cn.Query<Inventory>("GetInventoryBySchool", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void AddPurchaseOrder(PurchaseOrder po, int mpId)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", po.Username);
                parameters.Add("@TotalQty", po.TotalQty);
                parameters.Add("@ModelPartId", mpId);
                parameters.Add("@MPCostId", po.MPCostId);
                parameters.Add("@PONumber", po.PONumber);
                parameters.Add("@Notes", po.Notes);

                cn.Execute("AddPurchaseOrder", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditPONotes(long poNumber, string notes)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PONumber", poNumber);
                parameters.Add("@Notes", notes);

                cn.Execute("EditPONotes", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeletePurchaseOrder(long poNumber)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@PONumber", poNumber);

                return cn.Execute("DeletePurchaseOrder", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<PurchaseOrder> GetPurchaseOrders()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<PurchaseOrder>("GetPurchaseOrders", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<PurchaseOrder> GetPurchaseOrders(string username)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);

                return cn.Query<PurchaseOrder>("GetPurchaseOrdersByUsername", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void AddPurchaseOrderLI(PurchaseOrderLI li)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PONumber", li.PONumber);
                parameters.Add("@ModelPartId", li.ModelPartId);
                parameters.Add("@Qty", li.Qty);
                parameters.Add("@MPCostId", li.MPCostId);
                parameters.Add("@TotalPrice", li.TotalPrice);

                cn.Execute("AddPurchaseOrderLI", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddToPOLIQty(int listItemId, int qty)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@LineItemId", listItemId);
                parameters.Add("@Qty", qty);

                cn.Execute("AddToPOLIQty", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditPurchaseOrderLI(PurchaseOrderLI li)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@POLineItemId", li.POLineItemId);
                parameters.Add("@Qty", li.Qty);
                if(li.TotalPrice.HasValue) parameters.Add("@TotalPrice", li.TotalPrice.Value);
                else parameters.Add("@TotalPrice", null);
                if(li.DateReceived.HasValue) parameters.Add("@DateReceived", li.DateReceived.Value);
                else parameters.Add("@DateReceived", null);

                cn.Execute("EditPurchaseOrderLI", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeletePurchaseOrderLI(int lineItemId)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@LineItemId", lineItemId);

                return cn.Execute("DeletePurchaseOrderLI", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<PurchaseOrderLI> GetPurchaseOrderLIs(long poNumber)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PONumber", poNumber);

                return cn.Query<PurchaseOrderLI>("GetPurchaseOrderLIs", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public PurchaseOrderLI GetPurchaseOrderLI(int lineItemId)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@POLineItemId", lineItemId);

                return cn.QueryFirstOrDefault<PurchaseOrderLI>("GetPurchaseOrderLI", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void TransferInventory(int fromSource, int toSchool, int qty, string username, bool recycled)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@FromSource", fromSource);
                parameters.Add("@ToSchool", toSchool);
                parameters.Add("@Qty", qty);
                parameters.Add("@Username", username);
                parameters.Add("@Recycled", recycled);

                cn.Execute("TransferInventory", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<InventoryTransfer> GetInventoryTransfers()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<InventoryTransfer>("GetInventoryTransfers", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void AddNotification(string username, string notifMessage, string notifType, string fromUsername = null, int? schoolId = null, int? modelPartId = null, int? qty = null)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);
                parameters.Add("@NotifMessage", notifMessage);
                parameters.Add("@NotifType", notifType);
                parameters.Add("@FromUsername", fromUsername);
                parameters.Add("@SchoolId", schoolId.HasValue ? schoolId.Value : schoolId);
                parameters.Add("@ModelPartId", modelPartId.HasValue ? modelPartId.Value : modelPartId);
                parameters.Add("@Qty", qty.HasValue ? qty.Value : qty);

                cn.Execute("AddNotification", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditNotification(int id, string notifMessage, string notifType)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@NotificationId", id);
                parameters.Add("@NotifMessage", notifMessage);
                parameters.Add("@NotifType", notifType);

                cn.Execute("EditNotification", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool DeleteNotification(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@NotificationId", id);

                return cn.Execute("DeleteNotification", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool DeleteNotifications(string username)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);

                return cn.Execute("DeleteNotifications", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Notification> GetNotifications(string username)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);

                return cn.Query<Notification>("GetNotifications", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void NotificationsSeen(string username)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Username", username);
                cn.Execute("NotificationsSeen", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddUser(User user)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", user.Username);
                parameters.Add("@FirstName", user.FirstName);
                parameters.Add("@LastName", user.LastName);
                parameters.Add("@Email", user.Email);
                parameters.Add("@Role", user.Role);
                parameters.Add("@AddedBy", user.AddedBy);

                cn.Execute("AddUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditUser(User user)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", user.Username);
                parameters.Add("@Email", user.Email);
                parameters.Add("@Role", user.Role);

                cn.Execute("EditUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public User GetUserDetails(string username)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);

                return cn.QueryFirstOrDefault<User>("GetUserDetails", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<InvEditHistory> GetInvEditHistory()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<InvEditHistory>("GetInvEditHistory", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void EditInvEditNotes(int id, string notes)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@InvEditId", id);
                parameters.Add("@Notes", notes);

                cn.Execute("EditInvEditNotes", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<CostHistory> GetPartCostHistory(int id)
        {
            using(var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ModelPartId", id);
                return cn.Query<CostHistory>("GetPartCostHistory", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<CostHistory> GetModelCostHistory(int id)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ModelId", id);
                return cn.Query<CostHistory>("GetModelCostHistory", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Device GetStudentDeviceInfo(string username)
        {
            using (var cn = new SqlConnection(connString2))
            {
                string query = $@"select CopyBarcode as Barcode, SerialNumber, Title as ModelNumber, Price from CircCatAdmin.[Copy] c
	                            inner join CircCatAdmin.CopyAsset ca on c.CopyID = ca.CopyID
	                            inner join CircCatAdmin.BibMaster bm on bm.BibID = c.BibID
	                            inner join CircCatAdmin.Patron p on p.PatronID = c.PatronID
	                            inner join CircCatAdmin.Users u on u.UserID = p.UserID
	                            where LoginID = '{username}'
                                and Title like '%chromebook%'
                                and BibType = 0";

                return cn.QueryFirstOrDefault<Device>(query, commandType: CommandType.Text);
            }
        }

        public Device GetUnassignedDeviceInfo(string barcode)
        {
            using (var cn = new SqlConnection(connString2))
            {
                string query = $@"select CopyBarcode as Barcode, SerialNumber, Title as ModelNumber, LoginID as CurrentOwner, Price from CircCatAdmin.[Copy] c
	                            inner join CircCatAdmin.CopyAsset ca on c.CopyID = ca.CopyID
	                            inner join CircCatAdmin.BibMaster bm on bm.BibID = c.BibID
	                            left join CircCatAdmin.Patron p on p.PatronID = c.PatronID
	                            left join CircCatAdmin.Users u on u.UserID = p.UserID
	                            where CopyBarcode = '{barcode}'
                                and Title like '%chromebook%'
                                and BibType = 0";

                return cn.QueryFirstOrDefault<Device>(query, commandType: CommandType.Text);
            }
        }
    }
}
