namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdjustmentVoucherDetail")]
    public partial class AdjustmentVoucherDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AdjustmentVoucherID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InventoryID { get; set; }

        public int? AdjustmentQuantity { get; set; }

        [Column(TypeName = "text")]
        public string AdjustmentComments { get; set; }

        public virtual AdjustmentVoucher AdjustmentVoucher { get; set; }

        public virtual Inventory Inventory { get; set; }
    }
}
