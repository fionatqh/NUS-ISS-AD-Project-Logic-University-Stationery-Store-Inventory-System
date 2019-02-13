namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserRequestStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserRequestStatus()
        {
            UserRequest = new HashSet<UserRequest>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserRequestStatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserRequestStatusName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRequest> UserRequest { get; set; }
    }
}
