namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiscrepancyDetail")]
    public partial class DiscrepancyDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DiscrepancyID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InventoryID { get; set; }

        public decimal? Price { get; set; }

        public int InventoryQuantity { get; set; }

        public decimal? InventoryAmount { get; set; }

        public int ActualQuantity { get; set; }

        public decimal? ActualAmount { get; set; }

        public int? DiscrepancyQuantity { get; set; }

        public decimal? DiscrepancyAmount { get; set; }

        [Column(TypeName = "text")]
        public string DiscrepancyReason { get; set; }

        public virtual Discrepancy Discrepancy { get; set; }

        public virtual Inventory Inventory { get; set; }
    }
}
