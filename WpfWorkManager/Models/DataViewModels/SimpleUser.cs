using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWorkManager.Models.DataViewModels
{
    public class SimpleUser
    {
        public int UserId { get; set; }
        public string UserIssue { get; set; }
        public string IssueComent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string IssueReport { get; set; }
        public string IssueChange { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int IssueState { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
    }
}
