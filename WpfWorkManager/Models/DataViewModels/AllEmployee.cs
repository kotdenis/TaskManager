using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWorkManager.Models.DataViewModels
{
    public class AllEmployee
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public int IntRole { get; set; }
        public int? LeadeID { get; set; }
        public bool IsRetired { get; set; }
        public int Id { get; set; }
    }
}
