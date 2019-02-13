namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Disbursement")]
    public partial class Disbursement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Disbursement()
        {
            DisbursementDetail = new HashSet<DisbursementDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DisbursementID { get; set; }

        public int CollectionPoint { get; set; }

        public int DepartmentID { get; set; }

        [Required]
        [StringLength(50)]
        public string DisbursementStatus { get; set; }

        [StringLength(256)]
        public string CollectedByUserEmail { get; set; }

        public DateTime? DisbursementDate { get; set; }

        public virtual CollectionPoint CollectionPoint1 { get; set; }

        public virtual SSISUser SSISUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisbursementDetail> DisbursementDetail { get; set; }

        public int RetrievalID { get; set; }
    }
}
