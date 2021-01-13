namespace WpfWorkManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sprint")]
    public partial class Sprint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sprint()
        {
            Issues = new HashSet<Issue>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string SprintName { get; set; }

        public DateTime SprintStartDate { get; set; }

        public DateTime SprintEndDate { get; set; }

        public string SprintTotalReport { get; set; }

        public int SprintSate { get; set; }

        public string SprintDraft { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
