using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SSISLibrary
{
    public class CustomUserRequestDetails
    {
        public string Email { get; set; }
        public int UserRequestID { get; set; }
        public DateTime UserRequestDate { get; set; }
        public string UserRequestStatusName { get; set; }

    }

    public class GetRequestAndStatus
    {
        public List<CustomUserRequestDetails> RequestData { get; set; }
        public List<GetRequestAndStatusDetails> RequestDataDetails { get; set; }
    }


    public class GetRequestAndStatusDetails
    {
        public string UserRequestStatusName { get; set; }
        public int UserRequestID { get; set; }
        public int RequestQuantity { get; set; }
        public int InventoryID { get; set; }
        public string ItemName { get; set; }
        public decimal? Price { get; set; }
    }


    public class NewRequest
    {
        public NewRequestMaster NewRequestMaster { get; set; }
        public List<NewRequestQuantityDetails> NewRequestQuantityDetails { get; set; }
    }

    public class NewRequestMaster
    {
        public int UserRequestID { get; set; }
        public int DepartmentID { get; set; }
        public DateTime RequestDate { get; set; }
        public int RequestStatus { get; set; }
        public DateTime RequestStatusDate { get; set; }
        public string RequestStatusComment { get; set; }
        public string Email { get; set; }


    }

    public class NewRequestQuantityDetails
    {
        public int InventoryID { get; set; }
        public int RequestQuantity { get; set; }

    }

}

