namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DeptRequestDetail")]
    public partial class DeptRequestDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeptRequestID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InventoryID { get; set; }

        public int DeptRequestQuantity { get; set; }

        public int? DeptCollectedQuantity { get; set; }

        public virtual DeptRequest DeptRequest { get; set; }

        public virtual Inventory Inventory { get; set; }
    }
}
