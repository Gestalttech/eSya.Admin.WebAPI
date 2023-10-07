using System;
using System.Collections.Generic;

namespace eSya.Admin.DL.Entities
{
    public partial class GtEcapct
    {
        public GtEcapct()
        {
            GtEcapcds = new HashSet<GtEcapcd>();
        }

        public int CodeType { get; set; }
        public string CodeTyepDesc { get; set; } = null!;
        public string CodeTypeControl { get; set; } = null!;
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual ICollection<GtEcapcd> GtEcapcds { get; set; }
    }
}
