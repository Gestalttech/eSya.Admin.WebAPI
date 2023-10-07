using System;
using System.Collections.Generic;

namespace eSya.Admin.DL.Entities
{
    public partial class GtAddrhd
    {
        public int Isdcode { get; set; }
        public int StateCode { get; set; }
        public int CityCode { get; set; }
        public string Zipcode { get; set; } = null!;
        public string? Zipdesc { get; set; }
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
