namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DeptRequest")]
    public partial class DeptRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeptRequest()
        {
            DeptRequestDetail = new HashSet<DeptRequestDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeptRequestID { get; set; }

        public int DepartmentID { get; set; }

        public DateTime DeptRequestDate { get; set; }

        public int DeptRequestStatus { get; set; }

        public DateTime DeptRequestStatusDate { get; set; }

        [Column(TypeName = "text")]
        public string DeptRequestStatusComment { get; set; }

        public virtual Department Department { get; set; }

        public virtual DeptRequestStatus DeptRequestStatus1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeptRequestDetail> DeptRequestDetail { get; set; }
        public object RetrievalID { get; internal set; }
    }
}
