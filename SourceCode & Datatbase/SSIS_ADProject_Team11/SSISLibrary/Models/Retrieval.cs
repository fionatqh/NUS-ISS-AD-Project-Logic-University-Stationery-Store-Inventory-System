namespace SSISLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Retrieval")]
    public partial class Retrieval
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Retrieval()
        {
            RetrievalDetail = new HashSet<RetrievalDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RetrievalID { get; set; }

        [StringLength(256)]
        public string RetrieverEmail { get; set; }

        public DateTime RetrievalDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RetrievalDetail> RetrievalDetail { get; set; }

        public virtual SSISUser SSISUser { get; set; }
    }
}
