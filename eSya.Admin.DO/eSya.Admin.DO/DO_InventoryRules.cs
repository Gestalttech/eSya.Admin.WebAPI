using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.DO
{
    public class DO_InventoryRules
    {
        public string InventoryRuleId { get; set; }
        public string InventoryRuleDesc { get; set; }
        public int InventoryRule { get; set; }
        public bool ApplyToSrn { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
