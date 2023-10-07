using System;
using System.Collections.Generic;

namespace eSya.Admin.DL.Entities
{
    public partial class GtEciuom
    {
        public int UnitOfMeasure { get; set; }
        public string Uompurchase { get; set; } = null!;
        public string Uomstock { get; set; } = null!;
        public string Uompdesc { get; set; } = null!;
        public string Uomsdesc { get; set; } = null!;
        public decimal ConversionFactor { get; set; }
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
