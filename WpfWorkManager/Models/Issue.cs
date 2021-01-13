namespace WpfWorkManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Issue")]
    public partial class Issue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Issue()
        {
            IssueConditions = new HashSet<IssueCondition>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string TaskName { get; set; }

        [Required]
        public string Description { get; set; }

        public int EmployeeId { get; set; }

        public int SprintId { get; set; }

        [StringLength(200)]
        public string Comment { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Sprint Sprint { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IssueCondition> IssueConditions { get; set; }
    }
}
