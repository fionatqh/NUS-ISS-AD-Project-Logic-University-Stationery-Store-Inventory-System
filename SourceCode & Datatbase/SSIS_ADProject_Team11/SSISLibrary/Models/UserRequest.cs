namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserRequest")]
    public partial class UserRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserRequest()
        {
            UserRequestDetail = new HashSet<UserRequestDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserRequestID { get; set; }

        public int DepartmentID { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public DateTime RequestDate { get; set; }

        public int RequestStatus { get; set; }

        public DateTime? RequestStatusDate { get; set; }

        [Column(TypeName = "text")]
        public string RequestStatusComment { get; set; }

        public virtual Department Department { get; set; }

        public virtual SSISUser SSISUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRequestDetail> UserRequestDetail { get; set; }

        public virtual UserRequestStatus UserRequestStatus { get; set; }
    }
}
