using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SSISLibrary
{
    public class DeptHeadLibrary
    {
        private SSISDbModelContext ctx = new SSISDbModelContext();
        CommonFunctionLibrary cfl = new CommonFunctionLibrary();

        // get current department representative
        public string GetCurrentDeptRep(int deptId)
        {
            string emName = ctx.Department.Where(x => x.DepartmentID == deptId).Select(y => y.RepEmail).First();
            return emName;
        }

        // get current collection point
        public string GetCurrentCollectionPoint(int deptId)
        {
            string colPt = ctx.Department.Where(x => x.DepartmentID == deptId).Select(y => y.CollectionPoint.CollectionPointName).First();
            return colPt;
        }

        // get current department head
        public string GetCurrentDeptHead(int deptId)
        {
            string deptHead = ctx.Department.Where(x => x.DepartmentID == deptId).Select(y => y.HeadEmail).First();
            return deptHead;
        }

        // get list of pending requests
        public List<CustomPendingRequest> GetPendingRequest(int deptId)
        {
            var pendingRequests = from rd in ctx.UserRequest
                                  join pn in ctx.SSISUser on rd.Email equals pn.Email
                                  where rd.RequestStatus == 1 && pn.DepartmentID == deptId
                                  select new CustomPendingRequest { RequestDate = rd.RequestDate, UserRequestID = rd.UserRequestID, PersonName = pn.PersonName };

            List<CustomPendingRequest> prList = new List<CustomPendingRequest>();
            prList = pendingRequests.ToList();
            return pendingRequests.ToList();
        }
        
        // get pending request details
        public List<CustomPendingRequestDetail> GetPendingRequestDetail()
        {
            var pendingRequestDetail =
            from rd in ctx.UserRequest
            join pn in ctx.SSISUser on rd.Email equals pn.Email
            from c in ctx.Category
            join itn in ctx.Inventory on c.CategoryID equals itn.CategoryID
            join rq in ctx.UserRequestDetail on itn.InventoryID equals rq.InventoryID
            select new CustomPendingRequestDetail { RequestDate = rd.RequestDate, UserRequestID = rd.UserRequestID, PersonName = pn.PersonName, CategoryName = c.CategoryName, ItemName = itn.ItemName, UnitOfMeasure = itn.UnitOfMeasure, RequestQuantity = rq.RequestQuantity };

            List<CustomPendingRequestDetail> prdList = new List<CustomPendingRequestDetail>();
            prdList = pendingRequestDetail.ToList();

            return pendingRequestDetail.ToList();
        }

        // get pending request details by user request id
        public List<CustomPendingRequestDetail> GetPendingRequestDetailByUserRequestID(int userRequestID)
        {
                    var query1 =
                     from ur in ctx.UserRequestDetail

                     join urs in ctx.Inventory on ur.InventoryID equals urs.InventoryID

                     join vr in ctx.UserRequest on ur.UserRequestID equals vr.UserRequestID

                     where ur.UserRequestID == userRequestID

                     select new CustomPendingRequestDetail
                     {

                         RequestDate = vr.RequestDate,
                         UserRequestID = vr.UserRequestID,
                         PersonName = vr.SSISUser.PersonName,
                         CategoryName = urs.Category.CategoryName,
                         ItemName = urs.ItemName,
                         UnitOfMeasure = urs.UnitOfMeasure,
                         RequestQuantity = ur.RequestQuantity
                     };

                    return query1.ToList();
        }


        // get list of approved requests
        public List<CustomPendingRequest> GetApprovedRequest(int deptId)
        {
            var approvedRequests = from rd in ctx.UserRequest
                                  join pn in ctx.SSISUser on rd.Email equals pn.Email
                                  where rd.RequestStatus == 2 && pn.DepartmentID == deptId
                                  select new CustomPendingRequest { RequestDate = rd.RequestDate, UserRequestID = rd.UserRequestID, PersonName = pn.PersonName };

            List<CustomPendingRequest> arList = new List<CustomPendingRequest>();
            arList = approvedRequests.ToList();
            return approvedRequests.ToList();
        }

        // get approved request details
        public List<CustomPendingRequestDetail> GetApprovedRequestDetailByUserRequestID(int userRequestID)
        {
            var query1 =
             from ur in ctx.UserRequestDetail

             join urs in ctx.Inventory on ur.InventoryID equals urs.InventoryID

             join vr in ctx.UserRequest on ur.UserRequestID equals vr.UserRequestID

             where ur.UserRequestID == userRequestID

             select new CustomPendingRequestDetail
             {

                 RequestDate = vr.RequestDate,
                 UserRequestID = vr.UserRequestID,
                 PersonName = vr.SSISUser.PersonName,
                 CategoryName = urs.Category.CategoryName,
                 ItemName = urs.ItemName,
                 UnitOfMeasure = urs.UnitOfMeasure,
                 RequestQuantity = ur.RequestQuantity
             };

            return query1.ToList();
        }

        // get list of rejected requests
        public List<CustomPendingRequest> GetRejectedRequest(int deptId)
        {
            var rejectedRequests = from rd in ctx.UserRequest
                                   join pn in ctx.SSISUser on rd.Email equals pn.Email
                                   where rd.RequestStatus == 3 && pn.DepartmentID == deptId
                                   select new CustomPendingRequest { RequestDate = rd.RequestDate, UserRequestID = rd.UserRequestID, PersonName = pn.PersonName };

            List<CustomPendingRequest> rrList = new List<CustomPendingRequest>();
            rrList = rejectedRequests.ToList();
            return rejectedRequests.ToList();
        }

        // get rejected request detail by user request id
        public List<CustomPendingRequestDetail> GetRejectedRequestDetailByUserRequestID(int userRequestID)
        {
            var query1 =
             from ur in ctx.UserRequestDetail

             join urs in ctx.Inventory on ur.InventoryID equals urs.InventoryID

             join vr in ctx.UserRequest on ur.UserRequestID equals vr.UserRequestID

             where ur.UserRequestID == userRequestID

             select new CustomPendingRequestDetail
             {

                 RequestDate = vr.RequestDate,
                 UserRequestID = vr.UserRequestID,
                 PersonName = vr.SSISUser.PersonName,
                 CategoryName = urs.Category.CategoryName,
                 ItemName = urs.ItemName,
                 UnitOfMeasure = urs.UnitOfMeasure,
                 RequestQuantity = ur.RequestQuantity
             };

            return query1.ToList();
        }

        // get list of user request status
        public List<UserRequestStatus> GetUserRequestStatus()
        {
            List<UserRequestStatus> urList = new List<UserRequestStatus>();
            urList = ctx.UserRequestStatus.ToList();
            if (urList.Count > 0)
                return urList;
            else
                return null;
        }

        // approve request
        public void ApproveRequest(int userRequestID, string currentUser)
        {
            UserRequest requestStatus;
            string toEmail;
            requestStatus = ctx.UserRequest.SingleOrDefault(x => x.UserRequestID == userRequestID);
            toEmail = requestStatus.Email;

            if (requestStatus != null)
            {
                requestStatus.RequestStatus = 2;
                ctx.SaveChanges();
            }

            // make a new DeptRequest for the approved UserRequest
            DeptRequest newDeptRequest = new DeptRequest();
            newDeptRequest.DeptRequestID = ctx.DeptRequest.Max(p => p.DeptRequestID) + 1;
            newDeptRequest.DepartmentID = requestStatus.DepartmentID;
            newDeptRequest.DeptRequestDate = DateTime.Now;
            newDeptRequest.DeptRequestStatus = 1;
            newDeptRequest.DeptRequestStatusDate = DateTime.Now;
            newDeptRequest.DeptRequestStatusComment = "pending";
            ctx.DeptRequest.Add(newDeptRequest);
            ctx.SaveChanges();

            // make new DeptRequestDetails for the approved UserRequest
            List<UserRequestDetail> userRequestDetails = ctx.UserRequestDetail.Where(p => p.UserRequestID == requestStatus.UserRequestID).ToList();

            foreach (var userRequestDetail in userRequestDetails)
            {
                DeptRequestDetail deptRequestDetail = new DeptRequestDetail();
                deptRequestDetail.DeptRequestID = newDeptRequest.DeptRequestID;
                deptRequestDetail.InventoryID = userRequestDetail.InventoryID;
                deptRequestDetail.DeptRequestQuantity = userRequestDetail.RequestQuantity;
                ctx.DeptRequestDetail.Add(deptRequestDetail);
                ctx.SaveChanges();
            }

            // send email to employees whose requests have been approved
            string mailSubject = "Request Approval";
            string mailBody = "Your request with RequestID: " + userRequestID + " has been approved.";
            cfl.SendEmail(currentUser, toEmail, @"123qwe!@#QWE", mailSubject, mailBody);
        }

        // reject request
        public void RejectRequest(int userRequestID, string currentUser, string comment)
        {
            UserRequest requestStatus;
            string toEmail;
            requestStatus = ctx.UserRequest.SingleOrDefault(x => x.UserRequestID == userRequestID);
            toEmail = requestStatus.Email;

            if (requestStatus != null)
            {
                requestStatus.RequestStatus = 3;
                requestStatus.RequestStatusComment = comment;
                ctx.SaveChanges();
            }
            
            string mailSubject = "Request Rejection";
            string mailBody = "Your request with RequestID: " + userRequestID + comment + " has been rejected.";
            cfl.SendEmail(currentUser, toEmail, @"123qwe!@#QWE", mailSubject, mailBody);
        }

        //13 
        //public List<string> GetEmployeeName(string currentUser)
        //{
        //    var user = ctx.SSISUser.SingleOrDefault(x => x.Email == currentUser);
        //    var userDept = ctx.SSISUser.Where(x => x.DepartmentID == user.DepartmentID);
        //    int userDeptID = Convert.ToInt32(userDept);
        //    List<String> eList = new List<String>();
        //    eList = ctx.SSISUser.Select(x => x.PersonName).ToList();
        //    eList.Count();
        //    return eList;
        //}

        //13
        //public List<SSISUser> GetEmployeeNameByDeptID(int deptID, string currentUser)
        //{
        //    var user = ctx.SSISUser.SingleOrDefault(x => x.Email == currentUser);
        //    List<SSISUser> uList = ctx.SSISUser.Where(x => x.DepartmentID == deptID).ToList<SSISUser>();

        //    if (uList.Count > 0)
        //    {
        //        return uList;
        //    }

        //    else
        //    {
        //        return null;
        //    }
        //}
        //13 
        //public int GetDeptID( string currentUser)
        //{
        //    var user = ctx.SSISUser.SingleOrDefault(x => x.Email == currentUser);
        //    var userDept = ctx.SSISUser.Where(x => x.DepartmentID == user.DepartmentID);
        //    return Convert.ToInt32(userDept);
        //    //string eName = ctx.SSISUser.Select(x => x.PersonName).FirstOrDefault();
        //    //return eName;
        //}

        //13
        //public void AddDelegation(string email, string userOriginalRole, int delegateID, string delegateOriginalRole, DateTime startDate, DateTime endDate)
        //{
        //    Delegation delegation = new Delegation
        //    {
        //        Email = email,
        //        UserOriginalRole = userOriginalRole,
        //        DelegationID = delegateID,
        //        DelegateOriginalRole = delegateOriginalRole,
        //        StartDate = startDate,
        //        EndDate = endDate
        //    };
        //    ctx.Delegation.Add(delegation);
        //    SSISUser delegateUserRole = ctx.SSISUser.Where(p => p.Email == email).FirstOrDefault();
        //    delegateUserRole.RoleID = delegateID;
        //    ctx.SaveChanges();
        //}

        // change department representative
        public void ChangeDeptRep(int deptId, string newRepEmail, string currentUser)
        {
            Department department = ctx.Department.Where(p => p.DepartmentID == deptId).First();
            string oldDeptRepEmail = department.RepEmail;

            // set department's new repEmail 
            department.RepEmail = newRepEmail;

            // change oldDeptRep's role!
            SSISUser oldDeptRep = ctx.SSISUser.Where(p => p.Email == oldDeptRepEmail).FirstOrDefault();

            if (department.DepartmentID == 7)
                oldDeptRep.RoleID = 7; // if storeman, change old deptrep back to store clerk user role

            else
                oldDeptRep.RoleID = 4; // if not storeman, change old deptrep back to employee user role. 

            // change newDeptRep's role!
            SSISUser newDeptRep = ctx.SSISUser.Where(p => p.Email == newRepEmail).FirstOrDefault();
            if (department.DepartmentID == 7)
                newDeptRep.RoleID = 6; // if storeman, change new deptrep up to store supervisor user role (dept rep equivalent)
            else
                newDeptRep.RoleID = 3; // if not storeman, change new deptrep up to deptrep user role. 
            // save changes
            ctx.SaveChanges();

            // send email to all parties involved

            // send email to old Dept Rep
            string mailSubject1 = "You have been removed as a Department Representative";
            string mailBody1 = "Dear " + oldDeptRepEmail + ", You have been removed as a Department Representative.";
            cfl.SendEmail(currentUser, oldDeptRepEmail, @"123qwe!@#QWE", mailSubject1, mailBody1);

            // send email to new Dept Rep
            string mailSubject2 = "You have been appointed as a Department Representative";
            string mailBody2 = "Dear " + newRepEmail + ", You have been appointed as a Department Representative.";
            cfl.SendEmail(currentUser, newRepEmail, @"123qwe!@#QWE", mailSubject2, mailBody2);
        }

        // get list of SSIS users
        public List<SSISUser> GetSSISList(int deptId)
        {
            try
            {
                List<SSISUser> SSISList = ctx.SSISUser.Where(p => p.DepartmentID == deptId).ToList<SSISUser>();
                if (SSISList.Count > 0)
                {
                    return SSISList;
                }
                else return null;
            }
            catch (Exception )
            {
                return null;
            }
        }

        // get SSIS role id
        public int GetSSISRoleID(string delegateEmail)
        {

            try
            {
                List<SSISUser> suList = ctx.SSISUser.Where(p => p.Email == delegateEmail).ToList<SSISUser>();
                SSISUser SU = suList[0];
                int roleID = Convert.ToInt32(SU.RoleID);
                return roleID;
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                return 0;
            }
        }

        // add delegation
        public void AddDelegation(string currentUserEmail, string delegateEmail, DateTime startDate, DateTime endDate)
        {
            /*validations to be done
             ** 1.Make sure there is only one delegate depthead per dept
             * 2.Make sure the delegateoriginalRole stored in Delegation table is relevant while we want to restore delegate to his original role
             * 3.Make sure startDte less than end date and grater than current date.To ask Team if there is max. time for delegation period
             * 4.If max and min delegation time is in delegation task,to validate startdate and enddate accordingly
             ** 5.To Validate if the delegate is not already a dept rep
             */

            int delegationId = ctx.Delegation.Max(p => p.DelegationID) + 1;
            string userOriginalRole = GetSSISRoleID(currentUserEmail).ToString();
            string delegateOriginalRole = GetSSISRoleID(delegateEmail).ToString();
            int delegateRoleToUse = 0;

            // find current delegate RoleID
            switch (userOriginalRole)
            {
                case "2":
                    delegateRoleToUse = 8;
                    break;
                case "3":
                    delegateRoleToUse = 9;
                    break;
                case "5":
                    delegateRoleToUse = 10;
                    break;
                case "11":
                    delegateRoleToUse = 11;
                    break;
            }

            SSISUser deptHeadOrRepUser = ctx.SSISUser.Where(p => p.Email == currentUserEmail).FirstOrDefault();
            SSISUser delegateUser = ctx.SSISUser.Where(p => p.Email == delegateEmail).FirstOrDefault();
            SSISUser currentDelegateHeadIfAny = ctx.SSISUser.Where(p => p.DepartmentID == deptHeadOrRepUser.DepartmentID && p.RoleID == delegateRoleToUse).FirstOrDefault();

            // validations here
            if (deptHeadOrRepUser.DepartmentID == delegateUser.DepartmentID)
            {
                if (currentDelegateHeadIfAny == null)
                {
                    #region // make new Delegation and save into context
                    Delegation delegation = new Delegation
                    {
                        DelegationID = delegationId,
                        Email = currentUserEmail,
                        UserOriginalRole = userOriginalRole,
                        DelegateEmail = delegateEmail,
                        DelegateOriginalRole = delegateOriginalRole,
                        StartDate = startDate,
                        EndDate = endDate,
                    };
                    ctx.Delegation.Add(delegation);
                    ctx.SaveChanges();
                    #endregion
                }

                else
                {
                    // already have a delegate head
                }
            }

            else
            {
                // delegate user is not in same department as headOrRep user
            }

            // send email to delegate
            string mailSubject = "You have been appointed as a delegate";
            string mailBody = "Dear " + delegateUser.PersonName + ", you have been appointed as a delegate.";
            cfl.SendEmail(currentUserEmail, delegateEmail, @"123qwe!@#QWE", mailSubject, mailBody);
        }

        // get list of all collection points
        public List<CollectionPoint> GetAllCollectionPoint()
        {
            return ctx.CollectionPoint.ToList();
        }

        // change collection point
        public int ChangeCollectionPoint(int departmentID, int collectionPointID)
        {
            var dept = ctx.Department.SingleOrDefault(x => x.DepartmentID == departmentID);
            dept.CollectionPointID = collectionPointID;
            ctx.SaveChanges();
            return 1;
        }
    }
}
