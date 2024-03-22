using eSya.Admin.DL.Entities;
using eSya.Admin.DO;
using eSya.Admin.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.DL.Repository
{
    public class RulesRepository: IRulesRepository
    {
        private readonly IStringLocalizer<RulesRepository> _localizer;
        public RulesRepository(IStringLocalizer<RulesRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Inventory Rules
        public async Task<List<DO_InventoryRules>> GetInventoryRules()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcinvrs

                    .Select(i => new DO_InventoryRules
                    {
                        InventoryRuleId = i.InventoryRuleId,
                        InventoryRuleDesc = i.InventoryRuleDesc,
                        InventoryRule = i.InventoryRule,
                        ApplyToSrn = i.ApplyToSrn,
                        ActiveStatus = i.ActiveStatus
                    }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertInventoryRule(DO_InventoryRules inventoryRule)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_invRuleId = db.GtEcinvrs.Where(i => i.InventoryRuleId.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (is_invRuleId != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0079", Message = string.Format(_localizer[name: "W0079"]) };
                        }
                        var is_invRuledescExists = db.GtEcinvrs.Where(i => i.InventoryRuleDesc.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleDesc.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (is_invRuledescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0080", Message = string.Format(_localizer[name: "W0080"]) };
                        }

                        var inv_Rule = new GtEcinvr
                        {
                            InventoryRuleId = inventoryRule.InventoryRuleId,
                            InventoryRuleDesc = inventoryRule.InventoryRuleDesc,
                            InventoryRule = inventoryRule.InventoryRule,
                            ApplyToSrn = inventoryRule.ApplyToSrn,
                            FormId = inventoryRule.FormId,
                            ActiveStatus = inventoryRule.ActiveStatus,
                            CreatedBy = inventoryRule.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = inventoryRule.TerminalID
                        };
                        db.GtEcinvrs.Add(inv_Rule);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateInventoryRule(DO_InventoryRules inventoryRule)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_invRuledescExists = db.GtEcinvrs.Where(i => i.InventoryRuleDesc.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleDesc.ToUpper().Replace(" ", "")
                        && i.InventoryRuleId.ToUpper().Replace(" ", "") != inventoryRule.InventoryRuleId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (is_invRuledescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0080", Message = string.Format(_localizer[name: "W0080"]) };
                        }

                        GtEcinvr inv_Rule = db.GtEcinvrs.Where(i => i.InventoryRuleId.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (inv_Rule != null)
                        {
                            inv_Rule.InventoryRuleDesc = inventoryRule.InventoryRuleDesc;
                            inv_Rule.InventoryRule = inventoryRule.InventoryRule;
                            inv_Rule.ApplyToSrn = inventoryRule.ApplyToSrn;
                            inv_Rule.ActiveStatus = inventoryRule.ActiveStatus;
                            inv_Rule.ModifiedBy = inventoryRule.UserID;
                            inv_Rule.ModifiedOn = DateTime.Now;
                            inv_Rule.ModifiedTerminal = inventoryRule.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0081", Message = string.Format(_localizer[name: "W0081"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveInventoryRules(bool status, string InventoryId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcinvr inventory_rule = db.GtEcinvrs.Where(w => w.InventoryRuleId.ToUpper().Replace(" ", "") == InventoryId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (inventory_rule == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0081", Message = string.Format(_localizer[name: "W0081"]) };
                        }

                        inventory_rule.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region Unit of Measure
        public async Task<List<DO_UnitofMeasure>> GetUnitofMeasurements()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEciuoms

                                  .Select(u => new DO_UnitofMeasure
                                  {
                                      UnitOfMeasure = u.UnitOfMeasure,
                                      Uompurchase = u.Uompurchase,
                                      Uomstock = u.Uomstock,
                                      Uompdesc = u.Uompdesc,
                                      Uomsdesc = u.Uomsdesc,
                                      ConversionFactor = u.ConversionFactor,
                                      ActiveStatus = u.ActiveStatus
                                  }).OrderBy(o => o.UnitOfMeasure).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateUnitofMeasurement(DO_UnitofMeasure uoms)
        {
            try
            {
                if (uoms.UnitOfMeasure != 0)
                {
                    return await UpdateUnitofMeasurement(uoms);
                }
                else
                {
                    return await InsertUnitofMeasurement(uoms);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertUnitofMeasurement(DO_UnitofMeasure uoms)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEciuom isUompExists = db.GtEciuoms.FirstOrDefault(u => u.Uompurchase.ToUpper().Replace(" ", "") == uoms.Uompurchase.ToUpper().Replace(" ", "") &&
                        u.Uomstock.ToUpper().Replace(" ", "") == uoms.Uomstock.ToUpper().Replace(" ", ""));
                        if (isUompExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0082", Message = string.Format(_localizer[name: "W0082"]) };
                        }

                        int maxval = db.GtEciuoms.Select(c => c.UnitOfMeasure).DefaultIfEmpty().Max();
                        int uomId = maxval + 1;
                        var objuom = new GtEciuom
                        {
                            UnitOfMeasure = uomId,
                            Uompurchase = uoms.Uompurchase,
                            Uomstock = uoms.Uomstock,
                            Uompdesc = uoms.Uompdesc,
                            Uomsdesc = uoms.Uomsdesc,
                            ConversionFactor = uoms.ConversionFactor,
                            ActiveStatus = uoms.ActiveStatus,
                            FormId = uoms.FormId,
                            CreatedBy = uoms.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = uoms.TerminalID
                        };
                        db.GtEciuoms.Add(objuom);
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateUnitofMeasurement(DO_UnitofMeasure uoms)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEciuom isUompExists = db.GtEciuoms.FirstOrDefault(u => u.Uompurchase.ToUpper().Replace(" ", "") == uoms.Uompurchase.ToUpper().Replace(" ", "") &&
                       u.Uomstock.ToUpper().Replace(" ", "") == uoms.Uomstock.ToUpper().Replace(" ", "") && u.UnitOfMeasure != uoms.UnitOfMeasure);
                        if (isUompExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0082", Message = string.Format(_localizer[name: "W0082"]) };

                        }

                        GtEciuom objuoms = db.GtEciuoms.Where(x => x.UnitOfMeasure == uoms.UnitOfMeasure).FirstOrDefault();

                        if (objuoms == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0083", Message = string.Format(_localizer[name: "W0083"]) };
                        }

                        objuoms.Uompurchase = uoms.Uompurchase;
                        objuoms.Uomstock = uoms.Uomstock;
                        objuoms.Uompdesc = uoms.Uompdesc;
                        objuoms.Uomsdesc = uoms.Uomsdesc;
                        objuoms.ConversionFactor = uoms.ConversionFactor;
                        objuoms.ActiveStatus = uoms.ActiveStatus;
                        objuoms.ModifiedBy = uoms.UserID;
                        objuoms.ModifiedOn = DateTime.Now;
                        objuoms.ModifiedTerminal = uoms.TerminalID;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_UnitofMeasure> GetUOMPDescriptionbyUOMP(string uomp)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result = db.GtEciuoms.Where(u => u.Uompurchase.ToUpper().Replace(" ", "") == uomp.ToUpper().Replace(" ", "")).Select(x => new DO_UnitofMeasure
                    {
                        Uompdesc = x.Uompdesc
                    }).FirstOrDefaultAsync();


                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_UnitofMeasure> GetUOMSDescriptionbyUOMS(string uoms)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result = db.GtEciuoms.Where(u => u.Uomstock.ToUpper().Replace(" ", "") == uoms.ToUpper().Replace(" ", "")).Select(x => new DO_UnitofMeasure
                    {
                        Uomsdesc = x.Uomsdesc
                    }).FirstOrDefaultAsync();


                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveUnitofMeasure(bool status, int unitId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEciuom unit_mesure = db.GtEciuoms.Where(w => w.UnitOfMeasure == unitId).FirstOrDefault();
                        if (unit_mesure == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0083", Message = string.Format(_localizer[name: "W0083"]) };
                        }

                        unit_mesure.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
       

        #endregion
    }
}
