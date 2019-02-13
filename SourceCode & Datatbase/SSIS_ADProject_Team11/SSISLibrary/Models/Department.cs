namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Department")]
    public partial class Department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            DeptRequest = new HashSet<DeptRequest>();
            UserRequest = new HashSet<UserRequest>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DepartmentID { get; set; }

        [StringLength(4)]
        public string DeptCode { get; set; }

        [Required]
        [StringLength(50)]
        public string DeptName { get; set; }

        public int CollectionPointID { get; set; }

        [StringLength(256)]
        public string HeadEmail { get; set; }

        [StringLength(256)]
        public string RepEmail { get; set; }

        [StringLength(256)]
        public string ContactEmail { get; set; }

        [StringLength(10)]
        public string ContactPhone { get; set; }

        [StringLength(10)]
        public string ContactFax { get; set; }

        public virtual CollectionPoint CollectionPoint { get; set; }

        public virtual SSISUser SSISUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeptRequest> DeptRequest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRequest> UserRequest { get; set; }
    }
}
