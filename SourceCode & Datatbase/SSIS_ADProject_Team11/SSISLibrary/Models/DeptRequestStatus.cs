namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DeptRequestStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeptRequestStatus()
        {
            DeptRequest = new HashSet<DeptRequest>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeptRequestStatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string DeptRequestStatusName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeptRequest> DeptRequest { get; set; }
    }
}
