using eSya.Admin.DO;
using eSya.Admin.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Admin.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApplicationCodesController : ControllerBase
    {
        private readonly IApplicationCodesRepository _ApplicationCodesRepository;

        public ApplicationCodesController(IApplicationCodesRepository applicationCodesRepository)
        {
            _ApplicationCodesRepository = applicationCodesRepository;
        }

        #region Application Codes
        /// <summary>
        /// Get User Defined Code Type List.
        /// UI Reffered - CodeType, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserDefinedCodeTypesList()
        {
            var uct = await _ApplicationCodesRepository.GetUserDefinedCodeTypesList();
            return Ok(uct);
        }
        
        /// <summary>
        /// Get Application Codes.
        /// UI Reffered - ApplicationCodes, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesAsync()
        {
            var ac = await _ApplicationCodesRepository.GetApplicationCodes();
            return Ok(ac);
        }

        /// <summary>
        /// Get Application Codes for specific Code Type.
        /// UI Reffered - ApplicationCodes,AssetGroup
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesByCodeType(int codeType)
        {
            var ac = await _ApplicationCodesRepository.GetApplicationCodesByCodeType(codeType);
            return Ok(ac);
        }

        /// <summary>
        /// Insert into Application Codes Table
        /// UI Reffered - ApplicationCode,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoApplicationCodes(DO_ApplicationCodes obj)
        {
            var msg = await _ApplicationCodesRepository.InsertIntoApplicationCodes(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Application Codes Table
        /// UI Reffered - ApplicationCode,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateApplicationCodes(DO_ApplicationCodes obj)
        {
            var msg = await _ApplicationCodesRepository.UpdateApplicationCodes(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Active Or De Active Application code.
        /// UI Reffered - Application Code
        /// </summary>
        /// <param name="status-app_code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveApplicationCode(bool status, int app_code)
        {
            var ac = await _ApplicationCodesRepository.ActiveOrDeActiveApplicationCode(status, app_code);
            return Ok(ac);
        }
        #endregion
    }
}
