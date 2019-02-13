namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PurchaseOrder")]
    public partial class PurchaseOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseOrder()
        {
            PurchaseOrderDetail = new HashSet<PurchaseOrderDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PurchaseOrderID { get; set; }

        public int SupplierID { get; set; }

        public DateTime PurchaseOrderDate { get; set; }

        [StringLength(50)]
        public string DeliveryStatus { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Column(TypeName = "text")]
        public string Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
