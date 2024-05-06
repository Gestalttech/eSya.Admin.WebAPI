using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.DO
{
    public class DO_UnitofMeasure
    {
        public int UnitOfMeasure { get; set; }
        public int Uompurchase { get; set; }
        public int Uomstock { get; set; }
        public decimal ConversionFactor { get; set; }
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string? Uompdesc { get; set; }
        public string? Uomsdesc { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string FormId { get; set; }
    }
}
