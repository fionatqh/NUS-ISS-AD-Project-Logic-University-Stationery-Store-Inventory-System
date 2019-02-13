using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace SSISLibrary
{
    public class EmployeeLibrary
    {


        private SSISDbModelContext ctx = new SSISDbModelContext();
        CommonFunctionLibrary cfl = new CommonFunctionLibrary();
        //Get user requests
        public IQueryable<CustomUserRequestDetails> GetUserRequest(string userEmail)
        {
            var query =
                  from ur in ctx.UserRequest
                  join urs in ctx.UserRequestStatus on ur.RequestStatus equals urs.UserRequestStatusID
                  where ur.Email == userEmail
                  select new CustomUserRequestDetails { UserRequestID = ur.UserRequestID, UserRequestDate = ur.RequestDate, UserRequestStatusName = urs.UserRequestStatusName, Email = ur.Email };

            return query;

        }
        //get user requests on date search
        public IQueryable<CustomUserRequestDetails> GetUserRequest(DateTime StartDate, DateTime EndDate, string userEmail)
        {

            var query =
            from ur in ctx.UserRequest
            join urs in ctx.UserRequestStatus on ur.RequestStatus equals urs.UserRequestStatusID
            where DbFunctions.TruncateTime(ur.RequestDate) >= DbFunctions.TruncateTime(StartDate) &&
            DbFunctions.TruncateTime(ur.RequestDate) <= DbFunctions.TruncateTime(EndDate) && ur.Email == userEmail
            select new CustomUserRequestDetails { UserRequestID = ur.UserRequestID, UserRequestDate = ur.RequestDate, UserRequestStatusName = urs.UserRequestStatusName };
            return query;

        }
        //get user requests details
        public GetRequestAndStatus GetRequestAndStatus(int UserRequestID)
        {
            var query =
            from ur in ctx.UserRequest
            join urs in ctx.UserRequestStatus on ur.RequestStatus equals urs.UserRequestStatusID
            where ur.UserRequestID == UserRequestID
            select new CustomUserRequestDetails { UserRequestID = ur.UserRequestID, UserRequestDate = ur.RequestDate, UserRequestStatusName = urs.UserRequestStatusName };


            List<GetRequestAndStatusDetails> lst = new List<GetRequestAndStatusDetails>();
            if (query != null && query.Count() > 0)
            {
                foreach (var item in query)
                {
                    var query1 =
                     from ur in ctx.UserRequestDetail

                     join urs in ctx.Inventory on ur.InventoryID equals urs.InventoryID

                     join vr in ctx.UserRequest on ur.UserRequestID equals vr.UserRequestID

                     where ur.UserRequestID == item.UserRequestID

                     select new GetRequestAndStatusDetails
                     {

                         UserRequestStatusName = vr.UserRequestStatus.UserRequestStatusName,
                         UserRequestID = ur.UserRequestID,
                         InventoryID = ur.InventoryID,
                         RequestQuantity = ur.RequestQuantity,
                         ItemName = urs.ItemName,
                         Price = urs.Price,
                     };

                    GetRequestAndStatus obj = new GetRequestAndStatus
                    {
                        RequestData = query.ToList(),
                        RequestDataDetails = query1.ToList()
                    };
                    return obj;
                }
                return null;
            }
            else
            {
                return null;
            }

        }
        //change the request quantity
        public void ChangeRequestQuantity(int UserRequestID, int InventoryID, int RequestQuantity)
        {

            var result = ctx.UserRequestDetail.SingleOrDefault(b => b.UserRequestID == UserRequestID && b.InventoryID == InventoryID);
            if (result != null)
            {
                result.RequestQuantity = RequestQuantity;
                ctx.SaveChanges();
            }

        }
        //to get all the inventory
        public List<Inventory> GetInventory()
        {
            List<Inventory> lst = new List<Inventory>();

            lst = ctx.Inventory.ToList();
            return lst;
        }
        // to request stationery
        public void InsertRequest(NewRequest objNewRequest, int deptID)
        {
            // insert

            var query = ctx.UserRequest.Max(x => x.UserRequestID) + 1;

            var UserRequest = ctx.Set<UserRequest>();
            UserRequest.Add(new UserRequest
            {
                UserRequestID = query,
                DepartmentID=deptID,
                RequestDate = objNewRequest.NewRequestMaster.RequestDate,
                RequestStatusDate = objNewRequest.NewRequestMaster.RequestStatusDate,
                RequestStatus = objNewRequest.NewRequestMaster.RequestStatus,
                RequestStatusComment = objNewRequest.NewRequestMaster.RequestStatusComment,
                Email = objNewRequest.NewRequestMaster.Email
                
            }
            );
            ctx.SaveChanges();


            foreach (var item in objNewRequest.NewRequestQuantityDetails)
            {
                var UserRequestDetail = ctx.Set<UserRequestDetail>();
                UserRequestDetail.Add(new UserRequestDetail
                {
                    UserRequestID = query,
                    RequestQuantity = item.RequestQuantity,
                    InventoryID = item.InventoryID
                }
                );
                ctx.SaveChanges();
            }

            // send Email to DeptHead for request approval
            string userName = ctx.SSISUser.Single(p => p.Email == objNewRequest.NewRequestMaster.Email).PersonName;
            string mailSubject = "New User Request";
            string mailBody = "A new user request has been made by "+userName+".";
            string fromEmail = objNewRequest.NewRequestMaster.Email;



            // send to dept head
            SSISUser deptHead = ctx.SSISUser.Single(p => p.DepartmentID == deptID && (p.RoleID == 2 || p.RoleID == 5));
            cfl.SendEmail(fromEmail, deptHead.Email, @"123qwe!@#QWE", mailSubject, mailBody);
            // send to delegate head, if any
            SSISUser delegateHead = ctx.SSISUser.SingleOrDefault(p => p.DepartmentID == deptID && (p.RoleID == 8 || p.RoleID == 10));
            if (delegateHead != null)
                cfl.SendEmail(fromEmail, delegateHead.Email, @"123qwe!@#QWE", mailSubject, mailBody);
        }
    }
}


