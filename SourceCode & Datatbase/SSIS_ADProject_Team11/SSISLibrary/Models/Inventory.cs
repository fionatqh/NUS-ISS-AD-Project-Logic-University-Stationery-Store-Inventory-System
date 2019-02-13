namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Inventory")]
    public partial class Inventory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory()
        {
            AdjustmentVoucherDetail = new HashSet<AdjustmentVoucherDetail>();
            DeptRequestDetail = new HashSet<DeptRequestDetail>();
            DisbursementDetail = new HashSet<DisbursementDetail>();
            DiscrepancyDetail = new HashSet<DiscrepancyDetail>();
            PurchaseOrderDetail = new HashSet<PurchaseOrderDetail>();
            RetrievalDetail = new HashSet<RetrievalDetail>();
            UserRequestDetail = new HashSet<UserRequestDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InventoryID { get; set; }

        [Required]
        [StringLength(4)]
        public string ItemNumber { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        public int ReorderLevel { get; set; }

        public int ReorderQuantity { get; set; }

        [Required]
        [StringLength(50)]
        public string UnitOfMeasure { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public int? Supplier1ID { get; set; }

        public decimal? Supplier1Price { get; set; }

        public int? Supplier2ID { get; set; }

        public decimal? Supplier2Price { get; set; }

        public int? Supplier3ID { get; set; }

        public decimal? Supplier3Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdjustmentVoucherDetail> AdjustmentVoucherDetail { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeptRequestDetail> DeptRequestDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisbursementDetail> DisbursementDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiscrepancyDetail> DiscrepancyDetail { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RetrievalDetail> RetrievalDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRequestDetail> UserRequestDetail { get; set; }
    }
}
