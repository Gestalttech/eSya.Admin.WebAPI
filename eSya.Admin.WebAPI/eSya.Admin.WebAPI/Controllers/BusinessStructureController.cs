using eSya.Admin.DO;
using eSya.Admin.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Admin.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessStructureController : ControllerBase
    {
        private readonly IBusinessStructureRepository _BusinessStructureRepository;
        public BusinessStructureController(IBusinessStructureRepository businessStructureRepository)
        {
            _BusinessStructureRepository = businessStructureRepository;
        }
        #region Business Statutory
        /// <summary>
        /// Getting  Business Key List.
        /// UI Reffered - Business Statutory Dropdown
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var bkeys = await _BusinessStructureRepository.GetBusinessKey();
            return Ok(bkeys);

        }

        /// <summary>
        /// Getting  ISD Code by Busines Key.
        /// UI Reffered - Business Statutory Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetISDCodeByBusinessKey(int BusinessKey)
        {
            var isdcode = await _BusinessStructureRepository.GetISDCodeByBusinessKey(BusinessKey);
            return Ok(isdcode);

        }

        /// <summary>
        /// Getting  Business Statutory List.
        /// UI Reffered - Business Statutory Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStatutoryInformation(int BusinessKey, int isdCode)
        {
            var b_entities = await _BusinessStructureRepository.GetStatutoryInformation(BusinessKey, isdCode);
            return Ok(b_entities);

        }

        /// <summary>
        /// Insert Business Statutory .
        /// UI Reffered -Business Statutory
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateBusinessStatutory(List<DO_BusinessStatutoryDetails> sd)
        {
            var msg = await _BusinessStructureRepository.InsertOrUpdateBusinessStatutory(sd);
            return Ok(msg);

        }
        #endregion  
    }
}
