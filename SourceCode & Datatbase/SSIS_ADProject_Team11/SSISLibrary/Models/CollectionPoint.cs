namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CollectionPoint")]
    public partial class CollectionPoint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CollectionPoint()
        {
            Department = new HashSet<Department>();
            Disbursement = new HashSet<Disbursement>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CollectionPointID { get; set; }

        [Required]
        [StringLength(50)]
        public string CollectionPointName { get; set; }

        [Required]
        [StringLength(10)]
        public string CollectionTime { get; set; }

        [StringLength(256)]
        public string ClerkEmail { get; set; }

        public virtual SSISUser SSISUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Disbursement> Disbursement { get; set; }
    }
}
