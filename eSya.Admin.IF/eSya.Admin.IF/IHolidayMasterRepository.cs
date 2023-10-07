using eSya.Admin.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.IF
{
    public interface IHolidayMasterRepository
    {
        #region Holiday Master
        Task<List<DO_HolidayMaster>> GetHolidayByBusinessKey(int BusinessKey);
        Task<DO_ReturnParameter> InsertIntoHolidayMaster(DO_HolidayMaster obj);
        Task<DO_ReturnParameter> UpdateHolidayMaster(DO_HolidayMaster obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveHolidayMaster(DO_HolidayMaster obj);
        #endregion
    }
}
