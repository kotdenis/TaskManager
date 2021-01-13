namespace WpfWorkManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IssueCondition")]
    public partial class IssueCondition
    {
        public int Id { get; set; }

        public int IssueId { get; set; }

        public int IssueState { get; set; }

        public string IssueComment { get; set; }

        public DateTime? IssueStartDate { get; set; }

        public DateTime? IssueEndDate { get; set; }

        public string IssueReport { get; set; }

        public DateTime? IssueReportDate { get; set; }

        public DateTime? IssueReportChangeDate { get; set; }

        public string IssueReportChange { get; set; }

        public virtual Issue Issue { get; set; }
    }
}
