using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SSISLibrary
{  
    public partial class CustomDisbursementDetail
    {
        public int DisbursementID { get; set; }
        public string ItemNumber { get; set; }

        public string ItemName { get; set; }

        public DateTime? DisbursementDate { get; set; }
        public int DisbursementQuantity { get; set; }

        public int RequestedQuantity { get; set; }

        public String CollectionPoint { get; set; }

        public string Status { get; set; }
        
       
    }
}
