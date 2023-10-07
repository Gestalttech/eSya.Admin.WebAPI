using System;
using System.Collections.Generic;

namespace eSya.Admin.DL.Entities
{
    public partial class GtAddrdt
    {
        public int Isdcode { get; set; }
        public string Zipcode { get; set; } = null!;
        public int ZipserialNumber { get; set; }
        public string Area { get; set; } = null!;
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
