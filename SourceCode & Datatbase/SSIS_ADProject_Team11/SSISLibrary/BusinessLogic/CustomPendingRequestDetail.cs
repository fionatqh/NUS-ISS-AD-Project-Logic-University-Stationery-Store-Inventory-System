using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SSISLibrary
{   
    public class CustomPendingRequestDetail
    {
        //UserRequest
        public DateTime RequestDate { get; set; }
       
        [Key]
        public int UserRequestID { get; set; }

        //SSISUser
        public string PersonName { get; set; }

        //Inventory
        public string CategoryName { get; set; }

        public string ItemName { get; set; }

        public string UnitOfMeasure { get; set; }

        //UserRequestDetail
        public int RequestQuantity { get; set; }

    }
}
