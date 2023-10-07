using eSya.Admin.DO;
using eSya.Admin.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Admin.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CountryAreaController : ControllerBase
    {
        private readonly ICountryAreaRepository _CountryAreaRepository;

        public CountryAreaController(ICountryAreaRepository CountryAreaRepository)
        {
            _CountryAreaRepository = CountryAreaRepository;
        }

        #region Address Area Header

        /// <summary>
        /// Get Active States.
        /// UI Reffered - Address Area Header, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveStatesbyISDCode(int isdCode)
        {
            var states = await _CountryAreaRepository.GetActiveStatesbyISDCode(isdCode);
            return Ok(states);
        }

        /// <summary>
        /// Get Active Cities by ISD Code & State Codes Area.
        /// UI Reffered - Address Area Header, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveCitiesbyStateCode(int isdCode, int stateCode)
        {
            var cites = await _CountryAreaRepository.GetActiveCitiesbyStateCode(isdCode, stateCode);
            return Ok(cites);
        }


        /// <summary>
        /// Get Address Area Header.
        /// UI Reffered - Address Area Header, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAddressAreaHeader(int isdCode, int stateCode, int cityCode)
        {
            var addr = await _CountryAreaRepository.GetAddressAreaHeader(isdCode, stateCode, cityCode);
            return Ok(addr);
        }

        /// <summary>
        /// Insert into Address Area Header & Details Table 
        /// UI Reffered - Address Area Header,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoZipArea(DO_AddressHeader obj)
        {
            var msg = await _CountryAreaRepository.InsertIntoZipArea(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Address Area Header & Details Table 
        /// UI Reffered - Address Area Header,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateIntoZipArea(DO_AddressHeader obj)
        {
            var msg = await _CountryAreaRepository.UpdateIntoZipArea(obj);
            return Ok(msg);
        }
        #endregion

        #region Address Area Details
        /// <summary>
        /// Get Address Area Details.
        /// UI Reffered - Address Area Header, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAddressAreaDetails(int isdCode, int stateCode, int cityCode, string? zipCode)
        {
            var areas = await _CountryAreaRepository.GetAddressAreaDetails(isdCode, stateCode, cityCode, zipCode);
            return Ok(areas);
        }
        #endregion
    }
}
