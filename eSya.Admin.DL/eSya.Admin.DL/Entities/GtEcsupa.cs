using System;
using System.Collections.Generic;

namespace eSya.Admin.DL.Entities
{
    public partial class GtEcsupa
    {
        public int Isdcode { get; set; }
        public int StatutoryCode { get; set; }
        public int ParameterId { get; set; }
        public bool Action { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
