﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.DO
{
    public class DO_HolidayMaster
    {
        public int BusinessKey { get; set; }
        public int Year { get; set; }
        public string HolidayType { get; set; } = null!;
        public DateTime HolidayDate { get; set; }
        public string HolidayDesc { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public bool _status { get; set; }
    }
}
