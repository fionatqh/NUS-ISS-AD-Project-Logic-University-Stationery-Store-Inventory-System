using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSISLibrary
{
    public class StoreClerkLibrary
    {
        private SSISDbModelContext ctx = new SSISDbModelContext();

        //1.Update delivery order
        public List<string> getsuppliercodes(string itemname)
        {
            List<string> suppliercodes = new List<string>();

            string sql = "SELECT s.suppliercode FROM Supplier AS s, Inventory AS inv WHERE s.SupplierID = inv.Supplier1ID AND inv.ItemName = @ItemName OR inv.ItemName = '' UNION ALL SELECT s.suppliercode FROM Supplier AS s, Inventory AS inv WHERE s.SupplierID = inv.Supplier2ID AND inv.ItemName = @ItemName OR inv.ItemName = '' UNION ALL SELECT s.suppliercode FROM Supplier AS s, Inventory AS inv WHERE s.SupplierID = inv.Supplier3ID AND inv.ItemName = @ItemName OR inv.ItemName = ''";

            string constr = ConfigurationManager.ConnectionStrings["SSISDbModelContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ItemName", itemname);
                    cmd.Connection = con;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suppliercodes.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return suppliercodes;
        }

        //public DataTable RetrievalList(string colpoint)
        //{
        //    DataTable dt = new DataTable();
        //    string sql = "select DISTINCT r.RetrievalID, r.RetrievalDate, dept.Deptname, cp.collectionPointname, cp.CollectionTime FROM Disbursement d, CollectionPoint cp, Department dept, Retrieval r, DeptRequest dr WHERE dr.DeptRequestStatus = 2  AND dept.DepartmentID = d.DepartmentID AND dept.CollectionPointID = cp.CollectionPointID AND d.retrievalID = r.RetrievalID AND cp.CollectionPointName = @CollectionPointName GROUP BY r.RetrievalID, dept.DeptName, cp.CollectionPointName, cp.CollectionTime, r.RetrievalDate, d.disbursementStatus ORDER BY r.RetrievalDate";

        //    string constr = ConfigurationManager.ConnectionStrings["SSISDbModelContext"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sql))
        //        {
        //            cmd.Parameters.AddWithValue("@CollectionPointName", colpoint);
        //            cmd.Connection = con;
        //            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //            {
        //                sda.Fill(dt);
        //            }
        //        }
        //    }
        //    return dt;
        //}

        public List<int> GetDisbursementIDFromRetrievalID(int retrievalID)
        {
            List<Disbursement> disbursementFromRetrievalID = ctx.Disbursement.Where(p => p.RetrievalID == retrievalID).ToList();
            List<int> disbursementIDFromRetrievalID = new List<int>();

            foreach (var disbursement in disbursementFromRetrievalID)
            {
                disbursementIDFromRetrievalID.Add(disbursement.DisbursementID);
            }
            return disbursementIDFromRetrievalID;
        }

        public DataTable RetrievalList(string Retriever)
        {
            DataTable dt = new DataTable();
            string sql = "select * from Retrieval where RetrieverEmail = @Retriever";

            string constr = ConfigurationManager.ConnectionStrings["SSISDbModelContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@Retriever", Retriever);
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }



        #region MADE FOR MANAGE RETRIEVAL PG. BUT STILL IN PROCESS 
        //public List<CustomRetrievalDetailForClerk> GetRetrievalDetailFromDisbursement(int retrievalID, string colpoint)
        //{

        //    List<CustomRetrievalDetailForClerk> cList = (
        //                from rd in ctx.RetrievalDetail
        //                join inv in ctx.Inventory on rd.InventoryID equals inv.InventoryID
        //                join r in ctx.Retrieval on rd.RetrievalID equals r.RetrievalID
        //                join disb in ctx.Disbursement on r.RetrievalID equals disb.RetrievalID
        //                join c in ctx.CollectionPoint on disb.CollectionPoint equals c.CollectionPointID
        //                where rd.RetrievalID == retrievalID && c.CollectionPointName == colpoint
        //                select new CustomRetrievalDetailForClerk
        //                {
        //                    RetrievalID = r.RetrievalID,
        //                    InventoryID = inv.InventoryID,
        //                    ItemName = inv.ItemName,
        //                    CollectionPointName = c.CollectionPointName,
        //                    RetrievalDate = r.RetrievalDate,
        //                    RetrievalQuantity = rd.RetrievalQuantity,
        //                }).ToList();
        //    return cList;
        //}

        public List<CustomRetrievalDetailForClerk> GetRetrievalDetail(int retrievalID)
        {
            
            List<CustomRetrievalDetailForClerk> cList = (
                        from rd in ctx.RetrievalDetail
                        join r in ctx.Retrieval on rd.RetrievalID equals r.RetrievalID
                        
                        join inv in ctx.Inventory on rd.InventoryID equals inv.InventoryID
                        where rd.RetrievalID == retrievalID
                        select new CustomRetrievalDetailForClerk
                        {
                            RetrievalID = rd.RetrievalID,
                            ItemNumber = inv.ItemNumber,
                            ItemName = inv.ItemName,
                            Quantity = inv.Quantity,
                            RetrievalDate = r.RetrievalDate,
                            RetrievalQuantity = rd.RetrievalQuantity,
                           
                        }).ToList();
            return cList;
        }


        public List<RetrievalDetail> GetRetrievalDetailByRetrievalID(int retrievalID)
        {
            return ctx.RetrievalDetail.Where(p => p.RetrievalID == retrievalID).ToList();
        }

        public List<int> GetRetrievalQuantitiesByRetrievalID(int retrievalID)
        {
            List<RetrievalDetail> retrievalDetail = ctx.RetrievalDetail.Where(p => p.RetrievalID == retrievalID).ToList();
            List<int> retrievalQuantities = new List<int>();
            foreach (var item in retrievalDetail)
                retrievalQuantities.Add(item.RetrievalQuantity);
            return retrievalQuantities;
        }

        public int GetRetrievalQuantityByRetrievalIDAndInventoryID(int retrievalID, int inventoryID)
        {
            RetrievalDetail retrievalDetail = ctx.RetrievalDetail.Where(p => p.RetrievalID == retrievalID && p.InventoryID == inventoryID).FirstOrDefault();
            return retrievalDetail.RetrievalQuantity;
        }

        #endregion


        public Disbursement GetDisbursement(int disbursementID)
        {
            return ctx.Disbursement.Where(p => p.DisbursementID == disbursementID).FirstOrDefault();
        }

        public void UpdateRetrievalQuantity(int retrievalID, int invID, int retrievalQuantityChange)
        {
            RetrievalDetail rd = GetOneRetrievalDetail(retrievalID, invID);
            if (rd != null)
            {
                //cannot edit if already disbursed


                // change Retrieval Quantity
                rd.RetrievalQuantity += retrievalQuantityChange;
                // change Inventory Quantity
                ctx.Inventory.Where(p => p.InventoryID == rd.InventoryID).FirstOrDefault().Quantity -= retrievalQuantityChange;
                ctx.SaveChanges();
            }
        }

        private RetrievalDetail GetOneRetrievalDetail(int retrievalID, int invID)
        {
            RetrievalDetail listRetrievalDetail = ctx.RetrievalDetail.Where(p => p.RetrievalID == retrievalID && p.InventoryID == invID).FirstOrDefault();
            return listRetrievalDetail;
        }

        public DisbursementDetail GetOneDisbursementDetail(int disbursementID, int inventoryID)
        {
            DisbursementDetail listDisbursementDetail = ctx.DisbursementDetail.Where(p => p.DisbursementID == disbursementID && p.InventoryID == inventoryID).FirstOrDefault();
            return listDisbursementDetail;
        }

        public void MakeDisbursementFromProcessRequisitions(string department, int retrievalId)
        {
            Disbursement disb = new Disbursement();
            int disbursementID = ctx.Disbursement.Max(p => p.DisbursementID) + 1;
            disb.DisbursementID = disbursementID;
            disb.RetrievalID = retrievalId;
            disb.DepartmentID = ctx.Department.Where(p => p.DeptName == department).FirstOrDefault().DepartmentID;
            disb.CollectionPoint = ctx.Department.Where(p => p.DeptName == department).FirstOrDefault().CollectionPointID;
            disb.DisbursementStatus = "Pending";
            ctx.Disbursement.Add(disb);
            ctx.SaveChanges();
        }

        public void MakeDisbursementFromManageRetrievals(int retrievalId)
        {
            Disbursement disb = new Disbursement();
            int disbursementID = ctx.Disbursement.Max(p => p.DisbursementID) + 1;
            disb.DisbursementID = disbursementID;
            disb.RetrievalID = retrievalId;
            // disb.DepartmentID = ctx.Department.Where(p => p.DeptName == ).FirstOrDefault().DepartmentID;
            // disb.CollectionPoint = ctx.Disbursement.Where(p => p.CollectionPoint == ).FirstOrDefault().CollectionPointID;
            disb.DisbursementStatus = "Pending";
            ctx.Disbursement.Add(disb);
            ctx.SaveChanges();
        }

        public List<string> GetCollectionPointByClerkID(string clerkId)
        {
            //SELECT CollectionPointName FROM[CollectionPoint] as cp, [AspNetUserLogins] as anul WHERE cp.ClerkEmail = anul.Email
            List<CollectionPoint> listCollectionPt = ctx.CollectionPoint.Where(p => p.ClerkEmail == clerkId).ToList();
            List<string> collectionPt = new List<String>();
            foreach (var item in listCollectionPt)
            {
                collectionPt.Add(item.CollectionPointName);
            }

            return collectionPt;
        }

        public int getMaxRetrievalId(string loginUser, DateTime dt)
        {
            Retrieval retrieval = new Retrieval();
            retrieval.RetrievalDate = dt;
            retrieval.RetrievalID = ctx.Retrieval.Max(p => p.RetrievalID) + 1;
            retrieval.RetrieverEmail = loginUser;

            ctx.Retrieval.Add(retrieval);
            ctx.SaveChanges();

            return ctx.Retrieval.Max(p => p.RetrievalID);
        }

        public int getMaxDisbursmentId()
        {
            return ctx.Disbursement.Max(p => p.DisbursementID);
        }

        public List<string> CollectionPoints(string loginUser)
        {
            List<string> clist = new List<string>();

            var cPoints = ctx.CollectionPoint.Where(x => x.ClerkEmail == loginUser).ToList();
            foreach (var item in cPoints)
            {
                clist.Add(item.CollectionPointName);
            }
            return clist;
        }

        public void saveRetrievalDetails(int retrievalid, int inventoryID, int retrievalQuantity)
        {
            RetrievalDetail retrievalDetail = new RetrievalDetail();
            retrievalDetail.RetrievalID = retrievalid;
            retrievalDetail.InventoryID = inventoryID;
            retrievalDetail.RetrievalQuantity = retrievalQuantity;

            ctx.RetrievalDetail.Add(retrievalDetail);

            Inventory inv = ctx.Inventory.Where(x => x.InventoryID == inventoryID).FirstOrDefault();
            inv.Quantity -= retrievalQuantity;

            ctx.SaveChanges();
        }

        public void saveSumRetrievalDetails(int retrievalid, int inventoryID, int retrievalQuantity)
        {
            RetrievalDetail retrievalDetail = ctx.RetrievalDetail.Where(x => x.RetrievalID == retrievalid && x.InventoryID == inventoryID).FirstOrDefault();

            retrievalDetail.RetrievalQuantity += retrievalQuantity;

            // ctx.RetrievalDetail.Add(retrievalDetail);

            Inventory inv = ctx.Inventory.Where(x => x.InventoryID == inventoryID).FirstOrDefault();
            inv.Quantity -= retrievalQuantity;

            ctx.SaveChanges();
        }

        public void saveDisbursmentDetails(int inventoryID, int retrievalQuantity)
        {

            DisbursementDetail disbursementDetail = new DisbursementDetail();
            disbursementDetail.DisbursementID = ctx.Disbursement.Max(p => p.DisbursementID);
            disbursementDetail.InventoryID = inventoryID;
            disbursementDetail.DisbursementQuantity = retrievalQuantity;

            ctx.DisbursementDetail.Add(disbursementDetail);
            ctx.SaveChanges();
        }


        public DataTable PendingDeptOrdersSql(string colpoint)
        {
            DataTable dt = new DataTable();

            string sql = "select inv.InventoryID, inv.ItemName, inv.Quantity, inv.UnitOfMeasure, dept.DeptName, sum(drd.DeptRequestQuantity - drd.DeptCollectedQuantity) as [Pending_Quantity], min(dr.DeptRequestDate) as [Earliest_Request_Date] FROM Inventory as inv, DeptRequestDetail as drd, Department as dept, DeptRequest as dr, CollectionPoint as CP WHERE dr.DeptRequestStatus = 1 and inv.InventoryID = drd.InventoryID and drd.DeptRequestID = dr.DeptRequestID and dept.DepartmentID = dr.DepartmentID and dept.CollectionPointID = cp.CollectionPointID and (cp.CollectionPointName = @CollectionPointName or cp.CollectionPointName = '') GROUP BY inv.inventoryID, inv.itemname, inv.Quantity, inv.unitofmeasure, dept.DeptName ORDER BY inv.ItemName, [Earliest_Request_Date]";

            string constr = ConfigurationManager.ConnectionStrings["SSISDbModelContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@CollectionPointName", colpoint);
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable DisbursementDetailsFromRetrievalID(int retrievalID)
        {
            DataTable dt = new DataTable();

            //string sql = "select inv.InventoryID, inv.ItemName, inv.Quantity, inv.UnitOfMeasure, dept.DeptName, sum(drd.DeptRequestQuantity - drd.DeptCollectedQuantity) as [Pending_Quantity], min(dr.DeptRequestDate) as [Earliest_Request_Date] FROM Inventory as inv, DeptRequestDetail as drd, Department as dept, DeptRequest as dr, CollectionPoint as CP WHERE dr.DeptRequestStatus = 1 and inv.InventoryID = drd.InventoryID and drd.DeptRequestID = dr.DeptRequestID and dept.DepartmentID = dr.DepartmentID and dept.CollectionPointID = cp.CollectionPointID and (cp.CollectionPointName = @CollectionPointName or cp.CollectionPointName = '') GROUP BY inv.inventoryID, inv.itemname, inv.Quantity, inv.unitofmeasure, dept.DeptName ORDER BY inv.ItemName, [Earliest_Request_Date]";

            string sql = "select inv.InventoryID, inv.ItemName, inv.Quantity, inv.UnitOfMeasure, dept.DeptName, disb.DisbursementID, dd.DisbursementQuantity FROM Inventory as inv, DisbursementDetail as dd, Disbursement as disb, Department as dept WHERE dd.InventoryID = inv.InventoryID and disb.DepartmentID = dept.DepartmentID and dd.DisbursementID = disb.DisbursementID and disb.RetrievalID = @RetrievalID";

            string constr = ConfigurationManager.ConnectionStrings["SSISDbModelContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@RetrievalID", retrievalID);
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }

        

        //public DataTable PendingDeptOrdersSql_old(string colpoint)
        //{
        //    DataTable dt = new DataTable();

        //    string sql = "select inv.inventoryID, inv.itemname, inv.unitofmeasure, sum(drd.deptrequestquantity) as Request_Qty, dept.DeptName, inv.Quantity, dr.DeptRequestDate FROM inventory as inv, DeptRequestDetail as drd, Department as dept, DeptRequest as dr, CollectionPoint as CP WHERE (dr.deptRequestStatus = 1 and inv.InventoryID = drd.InventoryID and drd.DeptRequestID = dr.DeptRequestID and dept.DepartmentID = dr.DepartmentID and dept.CollectionPointID = cp.CollectionPointID and cp.CollectionPointName = 'Stationery Store - Administration Building') GROUP BY inv.inventoryID, inv.ItemName, inv.UnitOfMeasure, inv.Quantity, dept.DeptName, dr.DeptRequestDate ORDER BY inv.ItemName, dr.DeptRequestDate";

        //    string constr = ConfigurationManager.ConnectionStrings["SSISDbModelContext"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sql))
        //        {
        //            cmd.Parameters.AddWithValue("@CollectionPointName", colpoint);
        //            cmd.Connection = con;
        //            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //            {
        //                sda.Fill(dt);
        //            }
        //        }
        //    }
        //    return dt;
        //}

        // WORKING QUERIES FOR PendingDeptOrderSql:
        // NO DATE, SUM(DeptRequestQuantity)
        // select inv.inventoryID, inv.itemname, inv.Quantity, inv.unitofmeasure, dept.DeptName, sum(drd.deptrequestquantity) FROM inventory as inv, DeptRequestDetail as drd, Department as dept, DeptRequest as dr, CollectionPoint as CP WHERE dr.deptRequestStatus = 1 and inv.InventoryID = drd.InventoryID and drd.DeptRequestID = dr.DeptRequestID and dept.DepartmentID = dr.DepartmentID and dept.CollectionPointID = cp.CollectionPointID and cp.CollectionPointName = 'Stationery Store - Administration Building' GROUP BY inv.inventoryID, inv.itemname, inv.Quantity, inv.unitofmeasure, dept.DeptName ORDER BY inv.ItemName

        // NO SUM(DeptRequestQuantity), HAVE DATE
        // select inv.inventoryID, inv.itemname, inv.Quantity, inv.unitofmeasure, dept.DeptName, drd.deptrequestquantity, dr.DeptRequestDate FROM inventory as inv, DeptRequestDetail as drd, Department as dept, DeptRequest as dr, CollectionPoint as CP WHERE dr.deptRequestStatus = 1 and inv.InventoryID = drd.InventoryID and drd.DeptRequestID = dr.DeptRequestID and dept.DepartmentID = dr.DepartmentID and dept.CollectionPointID = cp.CollectionPointID and cp.CollectionPointName = 'Stationery Store - Administration Building' GROUP BY inv.inventoryID, inv.itemname, inv.Quantity, inv.unitofmeasure, dept.DeptName, dr.DeptRequestDate ORDER BY dr.DeptRequestDate, inv.ItemName


        public List<CustomDeptRequest> PendingDeptOrders(string colpoint)
        {
            var query =
            from drd in ctx.DeptRequestDetail
            join inv in ctx.Inventory on drd.InventoryID equals inv.InventoryID
            join dr in ctx.DeptRequest on drd.DeptRequestID equals dr.DeptRequestID
            join dept in ctx.Department on dr.DepartmentID equals dept.DepartmentID
            join cp in ctx.CollectionPoint on dept.CollectionPointID equals cp.CollectionPointID
            where dr.DeptRequestStatus == 1 && cp.CollectionPointName == colpoint
            select new CustomDeptRequest
            {
                InventoryID = inv.InventoryID,
                ItemName = inv.ItemName,
                Quantity = inv.Quantity,
                UnitOfMeasure = inv.UnitOfMeasure,
                DeptName = dept.DeptName,
                DeptRequestQuantity = drd.DeptRequestQuantity,
                DeptRequestDate = dr.DeptRequestDate
            };

            return query.ToList<CustomDeptRequest>();

        }



        //5. View Inventory Status
        public List<Inventory> ViewInventoryBelowReorderAsListInventory()
        {
            return ctx.Inventory.Where(x => x.Quantity < x.ReorderLevel).ToList();


            ////this is the sql query to use for this method.
            ////select * from Inventory, PurchaseOrder, PurchaseOrderDetail where Inventory.InventoryID = PurchaseOrderDetail.InventoryID and PurchaseOrderDetail.PurchaseOrderID = PurchaseOrder.PurchaseOrderID and PurchaseOrder.DeliveryStatus = 'Pending'
            //List<Inventory> InventoryBelowReorderAndNotPurchased = (from inven in ctx.Inventory
            //                     join pod in ctx.PurchaseOrderDetail on inven.InventoryID /* not */ equals pod.InventoryID
            //                     join po in ctx.PurchaseOrder on pod.PurchaseOrderID equals po.PurchaseOrderID
            //                     where po.DeliveryStatus == "Pending"
            //                     select inven).ToList();

            //string sql = "select InventoryID, ItemNumber, CategoryID, ItemName, ReorderLevel, ReorderQuantity, UnitOfMeasure, Quantity, Price, Supplier1ID, Supplier1Price, Supplier2ID, Supplier2Price, Supplier3ID, Supplier3Price from Inventory, PurchaseOrder, PurchaseOrderDetail where Inventory.InventoryID = PurchaseOrderDetail.InventoryID and PurchaseOrderDetail.PurchaseOrderID = PurchaseOrder.PurchaseOrderID and PurchaseOrder.DeliveryStatus = 'Pending'";

        }

        public List<Inventory> ViewAllInventory()
        {
            return ctx.Inventory.ToList();
        }

        public int GetInventoryIDFromItemNumber(string itemNumber)
        {
            return ctx.Inventory.Where(p=> p.ItemNumber == itemNumber).FirstOrDefault().InventoryID;
        }

        public List<PurchaseOrder> pendingPurchaseOrder()
        {
            return ctx.PurchaseOrder.Where(p => p.DeliveryStatus == "Pending").ToList();
        }

        public int GetMaxPurchaseOrderId()
        {
            return ctx.PurchaseOrder.Max(p => p.PurchaseOrderID);
        }

        public int CreatePurchaseOrder(string supplier)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            purchaseOrder.PurchaseOrderID = ctx.PurchaseOrder.Max(p => p.PurchaseOrderID) + 1;
            purchaseOrder.SupplierID = ctx.Supplier.Where(p => p.SupplierCode == supplier).First().SupplierID;
            purchaseOrder.PurchaseOrderDate = DateTime.Now;
            purchaseOrder.DeliveryStatus = "Pending";
            ctx.PurchaseOrder.Add(purchaseOrder);
            ctx.SaveChanges();

            return ctx.PurchaseOrder.Max(p => p.PurchaseOrderID);

        }

        public void CreatePurchaseOrderDetails(int pId, int quant, string itemName)
        {
            Inventory inventory = ctx.Inventory.Where(p => p.ItemName == itemName).First();

            PurchaseOrderDetail purchaseOrder = new PurchaseOrderDetail();
            purchaseOrder.PurchaseOrderID = pId;
            purchaseOrder.Quantity = quant;
            purchaseOrder.InventoryID = inventory.InventoryID;

            ctx.PurchaseOrderDetail.Add(purchaseOrder);
            ctx.SaveChanges();
        }

        public List<PurchaseOrderDetail> PendingPurchaseById(int pID)
        {
            return ctx.PurchaseOrderDetail.Where(p => p.PurchaseOrderID == pID).ToList();
        }

        public List<CustomDisbursementDetailForClerk> GetDisbursementDetail(int disbID) // get all Disbursement Detail by InventoryID
        {
            var users = from dd in ctx.DisbursementDetail
                        join inv in ctx.Inventory on dd.InventoryID equals inv.InventoryID
                        where dd.DisbursementID == disbID
                        join d in ctx.Disbursement on dd.DisbursementID equals d.DisbursementID
                        join c in ctx.CollectionPoint on d.CollectionPoint equals c.CollectionPointID

                        select new CustomDisbursementDetailForClerk
                        {
                            InventoryID = inv.InventoryID,
                            ItemName = inv.ItemName,
                            DisbursementDate = d.DisbursementDate,
                            DisbursementQuantity = dd.DisbursementQuantity,
                            CollectionPointName = c.CollectionPointName,
                            Status = d.DisbursementStatus,
                            DisbursementID = d.DisbursementID,
                        };
            return users.ToList();
        }

        public Retrieval GetRetrievalFromDisbursement(int disbID)
        {
            Disbursement disbursement = ctx.Disbursement.Where(p => p.DisbursementID == disbID).FirstOrDefault();
            Retrieval retrieval = ctx.Retrieval.Where(p => p.RetrievalID == disbursement.RetrievalID).FirstOrDefault();
            return retrieval;
        }

       

        //UPDATE DISBURSEMENT QTY
        public void UpdateDisbursementQuantity(int disbursementID, int inventoryID, int quantity)
        {
            DisbursementDetail dm = GetOneDisbursementDetail(disbursementID, inventoryID);
            if (dm != null)
            {

                dm.DisbursementQuantity = quantity;
                ctx.SaveChanges();

            }

        }
        //UPDATE DEPTREQUEST STATUS
        public void UpdateDeptRequestStatusTo3RdyForCollectionFetchByRetrievalID(int retrievalId)
        {
            Disbursement disbursement = ctx.Disbursement.Where(p => p.RetrievalID == retrievalId).FirstOrDefault();

            List<DeptRequest> listDeptRequest = ctx.DeptRequest.Where(p => p.DeptRequestStatus == 2 && p.DepartmentID == disbursement.DepartmentID).ToList();

            var updatedeptreqstat =
            (from dr in ctx.DeptRequest
             join disb in ctx.Disbursement on dr.DepartmentID equals disb.DepartmentID
             where disb.RetrievalID == retrievalId
             select dr).ToList();

            foreach (DeptRequest deptRequest in updatedeptreqstat)
            {
                if (deptRequest.DeptRequestStatus == 2)
                {
                    deptRequest.DeptRequestStatus = 3;
                }
            }

            ctx.SaveChanges();
        }


        public void UpdateDeptRequestStatusTo3RdyForCollection(string deptName)
        {
            Department dept = ctx.Department.Where(p => p.DeptName == deptName).FirstOrDefault();

            // List<DeptRequest> listDeptRequest = ctx.DeptRequest.Where(p => p.DeptRequestStatus == 1 && p.DepartmentID == dept.DepartmentID).ToList();

            var updatedeptreqstat =
            (from dr in ctx.DeptRequest
             join dep in ctx.Department on dr.DepartmentID equals dep.DepartmentID
             where dep.DeptName == deptName
             select dr).ToList();

            foreach (DeptRequest deptRequest in updatedeptreqstat)
            {
                if (deptRequest.DeptRequestStatus == 2)
                {
                    deptRequest.DeptRequestStatus = 3;
                }
            }

            ctx.SaveChanges();
        }


       



        public void UpdateDeptRequestStatusTo2Processing(string deptName)
        {
            Department dept = ctx.Department.Where(p => p.DeptName == deptName).FirstOrDefault();

            // List<DeptRequest> listDeptRequest = ctx.DeptRequest.Where(p => p.DeptRequestStatus == 1 && p.DepartmentID == dept.DepartmentID).ToList();

            var updatedeptreqstat =
            (from dr in ctx.DeptRequest
             join dep in ctx.Department on dr.DepartmentID equals dep.DepartmentID
             where dep.DeptName == deptName
             select dr).ToList();

            foreach (DeptRequest deptRequest in updatedeptreqstat)
            {
                if (deptRequest.DeptRequestStatus == 1)
                {
                    deptRequest.DeptRequestStatus =2;
                }
            }

            ctx.SaveChanges();
        }

        public DataTable PendingDisbursmentList(string colpoint)
        {
            DataTable dt = new DataTable();
            //string sql = "select DISTINCT d.disbursementID, r.RetrievalDate, dept.Deptname, cp.collectionPointname, cp.CollectionTime, d.disbursementStatus FROM Disbursement d, CollectionPoint cp, Department dept, Retrieval r WHERE d.DisbursementStatus = 'Pending'  AND dept.DepartmentID = d.DepartmentID AND dept.CollectionPointID = cp.CollectionPointID and cp.CollectionPointName = @CollectionPointName GROUP BY d.DisbursementID, dept.DeptName, cp.CollectionPointName, cp.CollectionTime, r.RetrievalDate, d.disbursementStatus ORDER BY r.RetrievalDate";

            string sql = "select DISTINCT d.disbursementID, r.RetrievalDate, dept.Deptname, cp.collectionPointname, cp.CollectionTime, d.disbursementStatus FROM Disbursement d, CollectionPoint cp, Department dept, Retrieval r WHERE d.DisbursementStatus = 'Pending'  AND dept.DepartmentID = d.DepartmentID AND dept.CollectionPointID = cp.CollectionPointID AND d.retrievalID = r.RetrievalID AND cp.CollectionPointName = @CollectionPointName GROUP BY d.DisbursementID, dept.DeptName, cp.CollectionPointName, cp.CollectionTime, r.RetrievalDate, d.disbursementStatus ORDER BY r.RetrievalDate";

            string constr = ConfigurationManager.ConnectionStrings["SSISDbModelContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@CollectionPointName", colpoint);
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }


        #region MyRegion

        public PurchaseOrder GetPurchaseOrder(int purchaseOrderID)
        {
            List<PurchaseOrder> purchaseOrder = ctx.PurchaseOrder.Where(p => p.PurchaseOrderID == purchaseOrderID).ToList();
            if (purchaseOrder.Count > 0)
                return purchaseOrder[0];
            else
                return null;
        }
        public List<DeliveryOrderDetail> GetDeliveryOrderDetailByPurchaseOrderID(int PurchaseOrderID)
        {
            var DeliveryRequestDetail =
            from po in ctx.PurchaseOrder
            join pa in ctx.PurchaseOrderDetail on po.PurchaseOrderID equals pa.PurchaseOrderID
            where po.PurchaseOrderID == PurchaseOrderID
            select new DeliveryOrderDetail { PurchaseOrderID = po.PurchaseOrderID, SupplierID = po.SupplierID, PurchaseOrderDate = po.PurchaseOrderDate, DeliveryStatus = po.DeliveryStatus, DeliveryDate = po.DeliveryDate, Comments = po.Comments, Quantity = pa.Quantity };

            List<DeliveryOrderDetail> prdList = new List<DeliveryOrderDetail>();
            prdList = DeliveryRequestDetail.ToList();

            return DeliveryRequestDetail.ToList();
        }
        public List<PurchaseOrder> GetPurchaseOrderDetails(int purchaseOrderID)
        {
            List<PurchaseOrder> purchaseOrder = ctx.PurchaseOrder.Where(p => p.PurchaseOrderID == purchaseOrderID).ToList();
            if (purchaseOrder.Count > 0)
                return purchaseOrder;
            else
                return null;
        }

        public List<PurchaseOrder> GetAllPurchaseOrder()
        {
            return ctx.PurchaseOrder.ToList<PurchaseOrder>();
        }

        public int UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            PurchaseOrder po = GetPurchaseOrder(purchaseOrder.PurchaseOrderID);
            if (po != null)
            {
                po.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
                po.SupplierID = purchaseOrder.SupplierID;
                po.PurchaseOrderDate = purchaseOrder.PurchaseOrderDate;
                po.DeliveryStatus = purchaseOrder.DeliveryStatus;
                po.DeliveryDate = purchaseOrder.DeliveryDate;
                po.Comments = purchaseOrder.Comments;
                ctx.SaveChanges();
                return 1;
            }
            return 0;
        }


        //6.Create discrepancy report
        public void DeleteDiscrepancy(int discrepancyID)
        {
            DiscrepancyDetail discrepancyDetail = ctx.DiscrepancyDetail.Where(p => p.DiscrepancyID == discrepancyID).FirstOrDefault();
            ctx.DiscrepancyDetail.Remove(discrepancyDetail);
            ctx.SaveChanges();
        }

        public void AddDiscrepancy()
        {
            Discrepancy discrepancy = new Discrepancy();
            discrepancy.DiscrepancyStatus = "Pending";
            discrepancy.DiscrepancyDate = DateTime.Now;
            ctx.Discrepancy.Add(discrepancy);
            ctx.SaveChanges();
        }

        public void AddDiscrepancyDetail(Discrepancy discrepancy, int inventoryID, decimal price, int inventoryQuantity, int actualQuantity, string discrepancyReason)
        {
            int discrepancyID = discrepancy.DiscrepancyID;
            DiscrepancyDetail discrepancyDetail = new DiscrepancyDetail
            {
                DiscrepancyID = discrepancyID,
                InventoryID = inventoryID,
                Price = price,
                InventoryQuantity = inventoryQuantity,
                ActualQuantity = actualQuantity,
                DiscrepancyReason = discrepancyReason
            };
            ctx.DiscrepancyDetail.Add(discrepancyDetail);
            ctx.SaveChanges();
        }

        public List<DiscrepancyDetail> GetAllDiscrepancy()
        {
            return ctx.DiscrepancyDetail.ToList<DiscrepancyDetail>();
        }

        public DiscrepancyDetail GetOneDiscrepancyDetail(int discrepancyID, int inventoryID)
        {
            DiscrepancyDetail listDiscrepancyDetail = ctx.DiscrepancyDetail.Where(p => p.DiscrepancyID == discrepancyID && p.InventoryID == inventoryID).FirstOrDefault();
            return listDiscrepancyDetail;
        }

        public void UpdateDiscrepancy(DiscrepancyDetail discrepancyDetail, int quantity, string reason)
        {
            discrepancyDetail.ActualQuantity = quantity;
            discrepancyDetail.DiscrepancyReason = reason;

            ctx.SaveChanges();
        }

        public int UpdateDiscrepancyQuantity(int discrepancyID, int inventoryID, int quantity, string reason)
        {
            DiscrepancyDetail dm = GetOneDiscrepancyDetail(discrepancyID, inventoryID);
            if (dm != null)
            {
                dm.ActualQuantity = quantity;
                dm.DiscrepancyReason = reason;
                ctx.SaveChanges();
                return 1;
            }
            return 0;
        }

        //2.Generate disbursements list
        public List<Disbursement> GetAllDisbursement()
        {
            return ctx.Disbursement.ToList<Disbursement>();
        }


        public int UpdateDisbursementStatus(Disbursement disbursement) // update DisbursementStatus from a disbursement object
        {
            Disbursement dm = GetDisbursement(disbursement.DisbursementID);
            if (dm != null)
            {
                dm.DisbursementStatus = disbursement.DisbursementStatus;
                ctx.SaveChanges();
                return 1;
            }
            return 0;
        }


        //3. Generate Retrieval Form
        public List<int> GetPendingDeptRequestID()
        {
            List<DeptRequest> listPendingDeptRequest = ctx.DeptRequest.Where(p => p.DeptRequestStatus == 1).ToList<DeptRequest>();
            List<int> listPendingDeptRequestID = new List<int>();

            foreach (var item in listPendingDeptRequest)
            {
                listPendingDeptRequestID.Add(item.DeptRequestID);
            }
            return listPendingDeptRequestID;
        }

        public List<DeptRequestDetail> GetPendingDeptRequestDetail(List<int> listPendingDeptRequestID)
        {
            List<DeptRequestDetail> listPendingDeptRequestDetail = new List<DeptRequestDetail>();

            foreach (var id in listPendingDeptRequestID)
            {
                listPendingDeptRequestDetail.Concat(ctx.DeptRequestDetail.Where(p => p.DeptRequestID == id).ToList());
            }
            return listPendingDeptRequestDetail;
        }

        public List<DeptRequest> GetAllDeptRequest()
        {
            return ctx.DeptRequest.ToList();
        }

        #endregion

        // BINDING GRIDVIEW
        //public void BindGrid(string colpoint, System.Web.UI.WebControls.GridView gridview1)
        //{
        //    string sql = "select inv.itemname, inv.unitofmeasure, sum(drd.deptrequestquantity) as totalqty, dept.DeptName FROM inventory as inv, DeptRequestDetail as drd, Department as dept, DeptRequest as dr, CollectionPoint as CP WHERE inv.InventoryID = drd.InventoryID and drd.DeptRequestID = dr.DeptRequestID and dept.DepartmentID = dr.DepartmentID and dept.CollectionPointID = cp.CollectionPointID and cp.CollectionPointName = @CollectionPointName OR cp.CollectionPointName = '' GROUP BY inv.ItemName, inv.UnitOfMeasure, dept.deptname";

        //    //string sql = "SELECT DISTINCT inv.ItemName, inv.UnitOfMeasure, sum(deptrequestquantity) as totalqty, dept.DeptName, dr.DeptRequestDate, drd.DeptRequestQuantity FROM Inventory AS inv, Department AS dept, DeptRequest AS dr, DeptRequestDetail AS drd, CollectionPoint AS cp GROUP BY drd.inventoryid, inv.itemname, inv.inventoryid, cp.CollectionPointID, dept.CollectionPointID, dr.DeptRequestID, drd.DeptRequestID, dr.DepartmentID, dept.DepartmentID, inv.UnitOfMeasure, dept.DeptName, dr.DeptRequestDate, drd.DeptRequestQuantity, cp.CollectionPointName HAVING drd.inventoryid = inv.inventoryid AND cp.CollectionPointID = dept.CollectionPointID AND dr.DeptRequestID = drd.DeptRequestID AND dr.DepartmentID = dept.DepartmentID AND drd.InventoryID = inv.InventoryID AND cp.CollectionPointName =@CollectionPointName OR @CollectionPointName = ''";

        //    string constr = ConfigurationManager.ConnectionStrings["SSISDbModelContext"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sql))
        //        {
        //            cmd.Parameters.AddWithValue("@CollectionPointName", colpoint);
        //            cmd.Connection = con;
        //            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //            {
        //                using (DataTable dt = new DataTable())
        //                {
        //                    sda.Fill(dt);
        //                    gridview1.DataSource = dt;
        //                    gridview1.DataBind();
        //                }
        //            }
        //        }
        //    }
        //}


        //public List<Inventory> ViewInventoryBelowReorderAsListInventory()
        //{
        //    return ctx.Inventory.Where(x => x.Quantity < x.ReorderLevel).ToList();
        //}

        //public List<Inventory> ViewAllInventory()
        //{
        //    return ctx.Inventory.ToList();
        //}
        //public DisbursementDetail GetOneDisbursementDetail(int disbursementID, int inventoryID)
        //{
        //    DisbursementDetail listDisbursementDetail = ctx.DisbursementDetail.Where(p => p.DisbursementID == disbursementID && p.InventoryID == inventoryID).FirstOrDefault();
        //    return listDisbursementDetail;
        //}

        //public void UpdateDisbursementQuantity(DisbursementDetail disbursementDetail, int quantity)
        //{
        //    disbursementDetail.DisbursementQuantity = quantity;
        //    ctx.SaveChanges();
        //}

        //public int UpdateDisbursementQuantity(int disbursementID, int inventoryID, int quantity)
        //{
        //    DisbursementDetail dm = GetOneDisbursementDetail(disbursementID, inventoryID);
        //    if (dm != null)
        //    {
        //        dm.DisbursementQuantity = quantity;
        //        ctx.SaveChanges();
        //        return 1;
        //    }
        //    return 0;
        //}

        //6. Manage Disbursement List (After Retrieved, on the way to collection - to change qty otw)
        // BINDING DISB LIST to GRIDVIEW1

        //BINDING DISB LIST DETAILS to GRIDVIEW2 (show after selecting index on GridView1)

        //public List<DisbursementDetail> GetDisbursementDetail(int disbursementid)
        //{
        //    List<DisbursementDetail> listDisbursementDetail = ctx.DisbursementDetail.Where(p => p.DisbursementID == disbursementid).ToList<DisbursementDetail>();
        //    return listDisbursementDetail;
        //}
        //public Inventory getInventoryList(int inventoryID)
        //{
        //    var iList = ctx.Inventory.Where(p => p.InventoryID == inventoryID).FirstOrDefault();
        //    return iList;
        //}


    }
}
