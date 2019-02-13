namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Supplier")]
    public partial class Supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supplier()
        {
            Inventory = new HashSet<Inventory>();
            PurchaseOrder = new HashSet<PurchaseOrder>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SupplierID { get; set; }

        [StringLength(4)]
        public string SupplierCode { get; set; }

        [Required]
        [StringLength(200)]
        public string SupplierName { get; set; }

        [StringLength(200)]
        public string SupplierAddress { get; set; }

        [StringLength(50)]
        public string SupplierContactName { get; set; }

        [StringLength(10)]
        public string SupplierContactPhone { get; set; }

        [StringLength(10)]
        public string SupplierContactFax { get; set; }

        [StringLength(12)]
        public string GSTRegistrationNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory> Inventory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrder { get; set; }
    }
}
