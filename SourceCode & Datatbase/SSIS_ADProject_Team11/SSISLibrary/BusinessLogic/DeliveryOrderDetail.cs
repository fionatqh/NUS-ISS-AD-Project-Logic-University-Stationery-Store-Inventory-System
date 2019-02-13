using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SSISLibrary
{
    public class DeliveryOrderDetail
    {
        internal int SupplierID;

        public int PurchaseOrderID { get; set; }

        public int InventoryID { get; set; }

        public int Quantity { get; set; }
        public DateTime PurchaseOrderDate { get; internal set; }
        public string DeliveryStatus { get; internal set; }
        public DateTime? DeliveryDate { get; internal set; }
        public string Comments { get; internal set; }
    }
}
