using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.DO
{
    public class DO_Department
    {
        public int DeptCategory { get; set; }
        public int DeptId { get; set; }
        public string DeptDesc { get; set; } 
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? CategoryDesc { get; set; }
    }
}
