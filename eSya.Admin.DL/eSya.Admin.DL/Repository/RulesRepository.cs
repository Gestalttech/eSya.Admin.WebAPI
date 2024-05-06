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
    public class RulesRepository : IRulesRepository
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
        public async Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeTypeList(List<int> l_codeType)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcapcds
                        .Where(w => w.ActiveStatus
                        && l_codeType.Contains(w.CodeType))
                        .Select(r => new DO_ApplicationCodes
                        {
                            CodeType = r.CodeType,
                            ApplicationCode = r.ApplicationCode,
                            CodeDesc = r.CodeDesc
                        }).OrderBy(o => o.CodeDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_UnitofMeasure>> GetUnitofMeasurements()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var ds = db.GtEciuoms.Join
                    (db.GtEcapcds,
                    um => new { um.Uompurchase },
                    up => new { Uompurchase = up.ApplicationCode },
                    (um, up) => new { um, up }).
                    Join(db.GtEcapcds,
                    ums => new { ums.um.Uomstock },
                    us => new { Uomstock = us.ApplicationCode },
                    (ums, us) => new { ums, us })

               .Select(r => new DO_UnitofMeasure
               {
                   UnitOfMeasure = r.ums.um.UnitOfMeasure,
                   Uompurchase = r.ums.um.Uompurchase,
                   Uomstock = r.ums.um.Uomstock,
                   ConversionFactor = r.ums.um.ConversionFactor,
                   Uompdesc = r.ums.up.CodeDesc,
                   Uomsdesc = r.us.CodeDesc,
                   ActiveStatus = r.ums.um.ActiveStatus
               }).ToListAsync();
                    return await ds;
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
                        GtEciuom isUompExists = db.GtEciuoms.FirstOrDefault(u => u.Uompurchase == uoms.Uompurchase && u.Uomstock == uoms.Uomstock);
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
                            ConversionFactor = uoms.ConversionFactor,
                            UsageStatus=false,
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
                        GtEciuom isUompExists = db.GtEciuoms.FirstOrDefault(u => u.Uompurchase== uoms.Uompurchase && u.Uomstock == uoms.Uomstock && u.UnitOfMeasure != uoms.UnitOfMeasure);
                        if (isUompExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0082", Message = string.Format(_localizer[name: "W0082"]) };

                        }

                        GtEciuom objuoms = db.GtEciuoms.Where(x => x.UnitOfMeasure == uoms.UnitOfMeasure).FirstOrDefault();

                        if (objuoms == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0083", Message = string.Format(_localizer[name: "W0083"]) };
                        }
                        bool used = db.GtEciuoms.Any(x => x.UnitOfMeasure == uoms.UnitOfMeasure && x.UsageStatus);
                        if (used)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0129", Message = string.Format(_localizer[name: "W0129"]) };

                        }
                        objuoms.Uompurchase = uoms.Uompurchase;
                        objuoms.Uomstock = uoms.Uomstock;
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveUnitofMeasure(bool status, int unitId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool used = db.GtEciuoms.Any(x => x.UnitOfMeasure == unitId && x.UsageStatus);
                        if (used)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0129", Message = string.Format(_localizer[name: "W0129"]) };

                        }
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
