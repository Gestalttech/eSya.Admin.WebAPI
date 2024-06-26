﻿using eSya.Admin.DO;
using eSya.Admin.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Admin.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly IRulesRepository _rulesRepository;
        public RulesController(IRulesRepository rulesRepository)
        {
            _rulesRepository = rulesRepository;
        }

        #region Inventory Rules
        /// <summary>
        /// Getting  Inventory Rules List.
        /// UI Reffered - Inventory Rules Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetInventoryRules()
        {
            var inv_rules = await _rulesRepository.GetInventoryRules();
            return Ok(inv_rules);
        }
        /// <summary>
        /// Insert Inventory Rules .
        /// UI Reffered -Inventory Rules
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertInventoryRule(DO_InventoryRules inventoryRule)
        {
            var msg = await _rulesRepository.InsertInventoryRule(inventoryRule);
            return Ok(msg);

        }
        /// <summary>
        /// Update Inventory Rules .
        /// UI Reffered -Inventory Rules
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateInventoryRule(DO_InventoryRules inventoryRule)
        {
            var msg = await _rulesRepository.UpdateInventoryRule(inventoryRule);
            return Ok(msg);

        }
        /// <summary>
        /// Active Or De Active Inventory Rule.
        /// UI Reffered - Inventory Rule
        /// </summary>
        /// <param name="status-InventoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveInventoryRules(bool status, string InventoryId)
        {
            var msg = await _rulesRepository.ActiveOrDeActiveInventoryRules(status, InventoryId);
            return Ok(msg);
        }
        #endregion

        #region Unit of Measure
        /// <summary>
        /// Getting  Unit of Purchase & Stock List.
        /// UI Reffered - Unit of Measure 
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GetApplicationCodesByCodeTypeList(List<int> l_codeType)
        {
            var msg = await _rulesRepository.GetApplicationCodesByCodeTypeList(l_codeType);
            return Ok(msg);

        }
        /// <summary>
        /// Getting  Unit of Measure List.
        /// UI Reffered - Unit of Measure Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUnitofMeasurements()
        {
            var unit_measures = await _rulesRepository.GetUnitofMeasurements();
            return Ok(unit_measures);
        }
        /// <summary>
        /// Insert Or Update Unit of Measure .
        /// UI Reffered -Unit of Measure
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateUnitofMeasurement(DO_UnitofMeasure uoms)
        {
            var msg = await _rulesRepository.InsertOrUpdateUnitofMeasurement(uoms);
            return Ok(msg);

        }
        
        /// <summary>
        /// Active Or De Active Unit of Measurement.
        /// UI Reffered - Unit of Measurement
        /// </summary>
        /// <param name="status-unitId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveUnitofMeasure(bool status, int unitId)
        {
            var msg = await _rulesRepository.ActiveOrDeActiveUnitofMeasure(status, unitId);
            return Ok(msg);
        }

        
        #endregion
    }
}
