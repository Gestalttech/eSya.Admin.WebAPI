using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.DO
{
    public class DO_CodeTypes
    {
        public int CodeType { get; set; }
        public string CodeTypeDesc { get; set; }
        public string CodeTypeControl { get; set; }
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
