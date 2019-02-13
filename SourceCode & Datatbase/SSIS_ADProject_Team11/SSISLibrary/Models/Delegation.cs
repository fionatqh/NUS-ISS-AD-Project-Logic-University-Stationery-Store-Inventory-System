namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Delegation")]
    public partial class Delegation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DelegationID { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(128)]
        public string UserOriginalRole { get; set; }

        [StringLength(256)]
        public string DelegateEmail { get; set; }

        [Required]
        [StringLength(128)]
        public string DelegateOriginalRole { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public virtual SSISUser SSISUser { get; set; }
    }
}
