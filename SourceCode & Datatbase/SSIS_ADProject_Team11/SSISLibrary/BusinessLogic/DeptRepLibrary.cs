using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Linq.Dynamic;

namespace SSISLibrary
{
    public class DeptRepLibrary
    {
        private SSISDbModelContext ctx = new SSISDbModelContext();
        CommonFunctionLibrary cfl = new CommonFunctionLibrary();

        public List<Disbursement> GetDisbursement(int deptID)
        {
            /*change*/

            List<Disbursement> disbursement = ctx.Disbursement.Where(p => p.DepartmentID == deptID).ToList<Disbursement>();
        
            if (disbursement.Count > 0)
            {
                return disbursement;
            }
            else return null;

        }

        public List<CustomDisbursementDetail> GetDisbursementDetail(int disbID) // get all Disbursement Detail by InventoryID
        {

            var users = from dd in ctx.DisbursementDetail
                        join inv in ctx.Inventory on dd.InventoryID equals inv.InventoryID
                        where dd.DisbursementID == disbID
                        join d in ctx.Disbursement on dd.DisbursementID equals d.DisbursementID
                        join c in ctx.CollectionPoint on d.CollectionPoint equals c.CollectionPointID //need to get,set in DisbursementDetails

                        select new CustomDisbursementDetail
                        {
                            ItemNumber = inv.ItemNumber,
                            ItemName = inv.ItemName,
                            DisbursementDate = d.DisbursementDate,
                            DisbursementQuantity = dd.DisbursementQuantity,
                            CollectionPoint = c.CollectionPointName,
                            Status = d.DisbursementStatus,
                            DisbursementID = d.DisbursementID    
                        }; // shows attributes from 3 different tables, thats why need so many joins. well done for figuring this out
            return users.ToList();


        }

        
            public int getReqQuantity(string InvName)
            {
                int dd,InvId;
                InvId = GetInvID(InvName);

                //int InvID = Convert.ToInt32(InvId);
                try
                {
                    dd = Convert.ToInt32((from vv in ctx.DeptRequestDetail
                                            where vv.InventoryID == InvId
                                            join dr in ctx.DeptRequest on vv.DeptRequestID equals dr.DeptRequestID
                                            where dr.DeptRequestStatus == 1
                                            group vv by vv.InventoryID into g
                                            select g.Sum(c => c.DeptRequestQuantity)).FirstOrDefault());

                }

                catch (Exception )
                {
                    return 0;
                }
                return dd;
            }

        public int ConfirmDisbursement(int disbID, int deptID, string currentUserEmail)
        {
            List<DisbursementDetail> dd = ctx.DisbursementDetail.Where(p => p.DisbursementID == disbID).ToList();
            DeptRequest thisDeptRequest = null;
            bool isDisbursed = false;
            
                foreach (DisbursementDetail item in dd)
                {
                    Disbursement d = ctx.Disbursement.Where(p => p.DisbursementID == item.DisbursementID).FirstOrDefault();

                    List<DeptRequest> dr = ctx.DeptRequest.Where(p => p.DepartmentID == deptID).ToList(); // get all dept requests for this department

                    DeptRequestDetail drd = null;

                    foreach (DeptRequest deptRequest in dr) // for all deptrequests in this department, find DeptRequestDetail for these deptrequests, with this requestItem, and pending quantity still uncollected
                    {
                        drd = ctx.DeptRequestDetail.Where(p => p.InventoryID == item.InventoryID && p.DeptCollectedQuantity < p.DeptRequestQuantity && p.DeptRequestID == deptRequest.DeptRequestID).FirstOrDefault();
                        if (drd != null)
                        {
                            thisDeptRequest = deptRequest;
                            break; // once deptrequestdetail and deptrequest is found, stop searching
                        }
                    }

                    if (thisDeptRequest != null)
                    {

                        isDisbursed = true; // if there is no deptrequestdetail, there will be no pending dept requests, this If block will not run and isDisbursed will not change.

                        if ((drd.DeptRequestQuantity - drd.DeptCollectedQuantity) > item.DisbursementQuantity)
                        {
                            drd.DeptCollectedQuantity += item.DisbursementQuantity;
                            //d.DisbursementStatus = "Disbursed";
                            thisDeptRequest.DeptRequestStatus = 1;
                            ctx.SaveChanges();
                        

                    }

                        else if ((drd.DeptRequestQuantity - drd.DeptCollectedQuantity) == item.DisbursementQuantity)
                        {
                            drd.DeptCollectedQuantity += item.DisbursementQuantity;
                            thisDeptRequest.DeptRequestStatus = 4;
                            ctx.SaveChanges();
                       
                    }

                        else if ((drd.DeptRequestQuantity - drd.DeptCollectedQuantity) < item.DisbursementQuantity)
                        {
                            drd.DeptCollectedQuantity = drd.DeptRequestQuantity;
                            thisDeptRequest.DeptRequestStatus = 4;


                            int disburseToNextRequest = (int)item.DisbursementQuantity - ((int)drd.DeptRequestQuantity - (int)drd.DeptCollectedQuantity);
                            DeptRequestDetail NextDrd = ctx.DeptRequestDetail.Where(p => p.InventoryID == item.InventoryID).OrderBy(q => q.DeptRequestID).Skip(1).Take(1).Single();
                            NextDrd.DeptCollectedQuantity += disburseToNextRequest;
                    
                            ctx.SaveChanges();
                            
                        }

                    }
               
                    if (isDisbursed == true)
                    {
                    // set disbursement status here to avoid setting it for every disbursementdetail
                        d.DisbursementStatus = "Disbursed";
                        d.DisbursementDate = DateTime.Now;
                        ctx.SaveChanges();

                        // Send Email to Store Clerk on Disbursement Received
                        string currentUserName = ctx.SSISUser.Where(p => p.Email == currentUserEmail).FirstOrDefault().PersonName;
                        int thisCollectionPointId = ctx.Department.Single(p => p.DepartmentID == thisDeptRequest.DepartmentID).CollectionPointID;
                        string toEmail = ctx.CollectionPoint.Single(p => p.CollectionPointID == thisCollectionPointId).ClerkEmail;
                        string mailSubject = "Disbursement Received";
                        string mailBody = "Your disbursement with DisbursementID: " + disbID + " was received by Department Rep: " + currentUserName + ".";
                        cfl.SendEmail(currentUserEmail, toEmail, @"123qwe!@#QWE", mailSubject, mailBody);
                        d.DisbursementDate = DateTime.Now;
                        return 1;
                    }
                    
                }

            if (isDisbursed)
            {
                
                return 1;
            }
            else
            {
                return 0;
            }


            
            
            

        }

        public int GetInvID(string InvName)
        {
            var dpt = (from p in ctx.Inventory where p.ItemName == InvName select p.InventoryID).Single();
            return Convert.ToInt32(dpt);
        }

        public Disbursement GetDisbursementbyID(int disbID)
        {
            var disb= (from p in ctx.Disbursement where p.DisbursementID== disbID select p).Single();
            return disb;


        }

        public List<Disbursement> GetDisbursementbyDate(DateTime stDate,DateTime endDate,int deptID)
        {
            try
            {
                var disb = (from p in ctx.Disbursement where p.DisbursementDate >= stDate && p.DisbursementDate <= endDate && p.DepartmentID == deptID select p).ToList();
                return disb;
                
            }
            catch
            {
                return null;
            }


        }

        public List<CollectionPoint> GetAllCollectionPoint()
        {
            return ctx.CollectionPoint.ToList();
        }

        public List<String> GetAllCollectionTimes()
        {
            var ct=from p in ctx.CollectionPoint select p.CollectionTime;
            return ct.ToList();
        }

        public String GetCollectionName(int collId)
        {
            var ct = ctx.CollectionPoint.SingleOrDefault(p => p.CollectionPointID == collId);
            return ct.CollectionPointName;
        }

        public int ChangeCollectionPoint(int departmentID, int collectionPointID)
        {
            try
            {
                var dept = ctx.Department.SingleOrDefault(x => x.DepartmentID == departmentID);
                dept.CollectionPointID = collectionPointID;
                ctx.SaveChanges();
            }
            catch
            {
                return 0;
            }
            return 1;
        }

        public int currentCollectionPoint(int departmentID)
        {
            var dpt = (from p in ctx.Department where p.DepartmentID == departmentID select p.CollectionPointID).Single();
            return Convert.ToInt32(dpt);
        }

        public String currentCollectionPointName(int collID)
        {
            var cpt = (from p in ctx.CollectionPoint where p.CollectionPointID== collID select p.CollectionPointName).Single();
            return cpt.ToString();
        }
        public int GetSSISRolelID(string currentUser)
        {
            try
            {
                var SU = (from p in ctx.SSISUser where p.Email == currentUser select p.RoleID).Single();
                return(Convert.ToInt32(SU));
                
            }
            catch (Exception )
            {
                return 0;
            }
        }

       
        public string GetDeptDisbStatus(int disbID)
        {
            try
            {
                var SU = (from p in ctx.Disbursement where p.DisbursementID == disbID select p.DisbursementStatus).Single();
                return SU.ToString();
                

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


      
            
        }
    }
