using eSya.Admin.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.IF
{
    public interface IBusinessStructureRepository
    {
        #region Business Statutory
        Task<List<DO_BusinessLocation>> GetBusinessKey();
        Task<DO_BusinessStatutoryDetails> GetISDCodeByBusinessKey(int BusinessKey);
        Task<List<DO_BusinessStatutoryDetails>> GetStatutoryInformation(int BusinessKey, int isdCode);
        Task<DO_ReturnParameter> InsertOrUpdateBusinessStatutory(List<DO_BusinessStatutoryDetails> obj);
        #endregion 
    }
}
