using eSya.Admin.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.IF
{
    public interface IApplicationCodesRepository
    {
        Task<List<DO_CodeTypes>> GetUserDefinedCodeTypesList();
        Task<List<DO_ApplicationCodes>> GetApplicationCodes();
        Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeType(int codeType);
        Task<DO_ReturnParameter> InsertIntoApplicationCodes(DO_ApplicationCodes obj);
        Task<DO_ReturnParameter> UpdateApplicationCodes(DO_ApplicationCodes obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveApplicationCode(bool status, int app_code);
    }
}
