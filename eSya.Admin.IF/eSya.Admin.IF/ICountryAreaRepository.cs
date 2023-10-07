using eSya.Admin.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.IF
{
    public interface ICountryAreaRepository
    {
        #region Address Area Header
        Task<List<DO_States>> GetActiveStatesbyISDCode(int isdCode);
        Task<List<DO_Cities>> GetActiveCitiesbyStateCode(int isdCode, int stateCode);
        Task<List<DO_AddressHeader>> GetAddressAreaHeader(int isdCode, int stateCode, int cityCode);
        Task<DO_ReturnParameter> InsertIntoZipArea(DO_AddressHeader obj);
        Task<DO_ReturnParameter> UpdateIntoZipArea(DO_AddressHeader obj);
        #endregion

        #region Address Area Details
        Task<List<DO_AddressDetails>> GetAddressAreaDetails(int isdCode, int stateCode, int cityCode, string zipCode);

        #endregion
    }
}
