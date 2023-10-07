using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.DO
{
    public class DO_AddressHeader
    {
        public int Isdcode { get; set; }
        public int StateCode { get; set; }
        public int CityCode { get; set; }
        public string Zipcode { get; set; }
        public string Zipdesc { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? StateDesc { get; set; }
        public string? CityDesc { get; set; }
        public List<DO_AddressDetails> _lstAddressdetails { get; set; }
    }
    public class DO_AddressDetails
    {
        public int Isdcode { get; set; }
        public string Zipcode { get; set; }
        public int ZipserialNumber { get; set; }
        public string Area { get; set; }
        public bool ActiveStatus { get; set; }
    }
    public class DO_States
    {
        public int Isdcode { get; set; }
        public int StateCode { get; set; }
        public string StateDesc { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
    public class DO_Cities
    {
        public int Isdcode { get; set; }
        public int StateCode { get; set; }
        public int CityCode { get; set; }
        public string CityDesc { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string StateDesc { get; set; }
    }
}
