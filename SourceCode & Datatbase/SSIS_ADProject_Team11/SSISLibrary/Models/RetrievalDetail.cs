namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RetrievalDetail")]
    public partial class RetrievalDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RetrievalID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InventoryID { get; set; }

        public int RetrievalQuantity { get; set; }

        public virtual Inventory Inventory { get; set; }

        public virtual Retrieval Retrieval { get; set; }
    }
}
