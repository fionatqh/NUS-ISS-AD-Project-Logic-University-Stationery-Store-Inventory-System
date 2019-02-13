namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdjustmentVoucher")]
    public partial class AdjustmentVoucher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdjustmentVoucher()
        {
            AdjustmentVoucherDetail = new HashSet<AdjustmentVoucherDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AdjustmentVoucherID { get; set; }

        public int? DiscrepancyID { get; set; }

        public DateTime? AdjustmentVoucherDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdjustmentVoucherDetail> AdjustmentVoucherDetail { get; set; }

        public virtual Discrepancy Discrepancy { get; set; }
    }
}
