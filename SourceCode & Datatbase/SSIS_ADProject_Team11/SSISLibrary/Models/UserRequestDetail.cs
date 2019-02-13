namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserRequestDetail")]
    public partial class UserRequestDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserRequestID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InventoryID { get; set; }

        public int RequestQuantity { get; set; }

        public virtual Inventory Inventory { get; set; }

        public virtual UserRequest UserRequest { get; set; }
    }
}
