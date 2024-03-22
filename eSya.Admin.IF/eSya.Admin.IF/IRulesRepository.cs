using eSya.Admin.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.IF
{
    public interface IRulesRepository
    {
        #region Inventory Rules
        Task<List<DO_InventoryRules>> GetInventoryRules();
        Task<DO_ReturnParameter> InsertInventoryRule(DO_InventoryRules inventoryRule);
        Task<DO_ReturnParameter> UpdateInventoryRule(DO_InventoryRules inventoryRule);
        Task<DO_ReturnParameter> ActiveOrDeActiveInventoryRules(bool status, string InventoryId);
        #endregion

        #region Unit of Measure
        Task<List<DO_UnitofMeasure>> GetUnitofMeasurements();
        Task<DO_ReturnParameter> InsertOrUpdateUnitofMeasurement(DO_UnitofMeasure uoms);
        Task<DO_ReturnParameter> InsertUnitofMeasurement(DO_UnitofMeasure uoms);
        Task<DO_ReturnParameter> UpdateUnitofMeasurement(DO_UnitofMeasure uoms);
        Task<DO_UnitofMeasure> GetUOMPDescriptionbyUOMP(string uomp);
        Task<DO_UnitofMeasure> GetUOMSDescriptionbyUOMS(string uoms);
        Task<DO_ReturnParameter> ActiveOrDeActiveUnitofMeasure(bool status, int unitId);
       
        #endregion
    }
}
