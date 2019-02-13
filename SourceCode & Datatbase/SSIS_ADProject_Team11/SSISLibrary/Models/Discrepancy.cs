namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Discrepancy")]
    public partial class Discrepancy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Discrepancy()
        {
            AdjustmentVoucher = new HashSet<AdjustmentVoucher>();
            DiscrepancyDetail = new HashSet<DiscrepancyDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DiscrepancyID { get; set; }

        [Required]
        [StringLength(50)]
        public string DiscrepancyStatus { get; set; }

        public DateTime DiscrepancyDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdjustmentVoucher> AdjustmentVoucher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiscrepancyDetail> DiscrepancyDetail { get; set; }
    }
}
