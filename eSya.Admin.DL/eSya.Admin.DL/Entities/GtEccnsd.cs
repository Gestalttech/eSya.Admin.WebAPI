using System;
using System.Collections.Generic;

namespace eSya.Admin.DL.Entities
{
    public partial class GtEccnsd
    {
        public int Isdcode { get; set; }
        public int StatutoryCode { get; set; }
        public string StatShortCode { get; set; } = null!;
        public string StatutoryDescription { get; set; } = null!;
        public string StatPattern { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
