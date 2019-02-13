using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SSISLibrary
{
    public partial class CustomDisbursementDetailForClerk
    {
        public int DisbursementID { get; set; }

        public int InventoryID { get; set; }

        public string ItemName { get; set; }

        public DateTime? DisbursementDate { get; set; }
        public int DisbursementQuantity { get; set; }

        public String CollectionPointName { get; set; }
        public string DeptName { get; set; }
        public string Status { get; set; }
    }

    public partial class CustomRetrievalDetailForClerk
    {
        public int RetrievalID { get; set; }
        public string ItemNumber { get; set; }
        public string ItemName { get; set; }
        public int? Quantity { get; set; }
        public DateTime RetrievalDate { get; set; }
        public int RetrievalQuantity { get; set; }
        
    }
}
