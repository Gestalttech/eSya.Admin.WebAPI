using eSya.Admin.DO;
using eSya.Admin.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Admin.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HolidayMasterController : ControllerBase
    {
        private readonly IHolidayMasterRepository _HolidayMasterRepository;

        public HolidayMasterController(IHolidayMasterRepository holidayMasterRepository)
        {
            _HolidayMasterRepository = holidayMasterRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetHolidayByBusinessKey(int BusinessKey)
        {
            var hb = await _HolidayMasterRepository.GetHolidayByBusinessKey(BusinessKey);
            return Ok(hb);
        }

        [HttpPost]
        public async Task<IActionResult> InsertIntoHolidayMaster(DO_HolidayMaster obj)
        {
            var msg = await _HolidayMasterRepository.InsertIntoHolidayMaster(obj);
            return Ok(msg);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHolidayMaster(DO_HolidayMaster obj)
        {
            var msg = await _HolidayMasterRepository.UpdateHolidayMaster(obj);
            return Ok(msg);

        }

        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActiveHolidayMaster(DO_HolidayMaster obj)
        {
            var ac = await _HolidayMasterRepository.ActiveOrDeActiveHolidayMaster(obj);
            return Ok(ac);

        }

    }
}
