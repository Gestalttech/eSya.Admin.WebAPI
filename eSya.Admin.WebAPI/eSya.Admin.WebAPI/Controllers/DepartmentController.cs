using eSya.Admin.DO;
using eSya.Admin.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Admin.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        #region Application Codes

        /// <summary>
        /// Get Categories from Application Codes.
        /// UI Reffered - Department, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDepartmentCategoriesbyCodeType(int codetype)
        {
            var ac = await _departmentRepository.GetDepartmentCategoriesbyCodeType(codetype);
            return Ok(ac);
        }


        /// <summary>
        /// Get Departments by categoryId.
        /// UI Reffered - Department, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDepartmentsbycategoryId(int categoryId)
        {
            var ac = await _departmentRepository.GetDepartmentsbycategoryId(categoryId);
            return Ok(ac);
        }
        /// <summary>
        /// Insert into Department Table
        /// UI Reffered - Department,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoDepartment(DO_Department obj)
        {
            var msg = await _departmentRepository.InsertIntoDepartment(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Department Table
        /// UI Reffered - Department,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(DO_Department obj)
        {
            var msg = await _departmentRepository.UpdateDepartment(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Active Or De Active Department.
        /// UI Reffered - Department
        /// </summary>
        /// <param name="status-categoryId-deptId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveDepartment(bool status, int categoryId, int deptId)
        {
            var ac = await _departmentRepository.ActiveOrDeActiveDepartment(status, categoryId, deptId);
            return Ok(ac);
        }
        #endregion
    }
}
