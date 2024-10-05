using System;
using System.Collections.Generic;

namespace eSya.Admin.DL.Entities
{
    public partial class GtAddrct
    {
        public int Isdcode { get; set; }
        public int StateCode { get; set; }
        public int CityCode { get; set; }
        public int Stdcode { get; set; }
        public string CityDesc { get; set; } = null!;
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
