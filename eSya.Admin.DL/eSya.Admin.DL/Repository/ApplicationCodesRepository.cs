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
    public class ApplicationCodesRepository: IApplicationCodesRepository
    {
        private readonly IStringLocalizer<ApplicationCodesRepository> _localizer;
        public ApplicationCodesRepository(IStringLocalizer<ApplicationCodesRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Application Codes
        public async Task<List<DO_CodeTypes>> GetUserDefinedCodeTypesList()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcapcts
                        .Where(w => w.ActiveStatus && w.CodeTypeControl == "U")
                        .Select(r => new DO_CodeTypes
                        {
                            CodeType = r.CodeType,
                            CodeTypeDesc = r.CodeTyepDesc
                        }).OrderBy(o => o.CodeTypeDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public async Task<List<DO_ApplicationCodes>> GetApplicationCodes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcapcds
                        .Select(r => new DO_ApplicationCodes
                        {
                            CodeType = r.CodeType,
                            ApplicationCode = r.ApplicationCode,
                            CodeDesc = r.CodeDesc,
                            ShortCode = r.ShortCode,
                            UsageStatus = r.UsageStatus,
                            DefaultStatus = r.DefaultStatus,
                            ActiveStatus = r.ActiveStatus,
                            TerminalID = r.CodeTypeNavigation.CodeTyepDesc
                        }).OrderBy(o => o.ApplicationCode).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeType(int codeType)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (codeType == 0)
                    {
                        var ds = await db.GtEcapcds.Join(db.GtEcapcts.Where(c => c.CodeTypeControl == "U"),
                       x => x.CodeType,
                       y => y.CodeType,
                       (x, y) => new DO_ApplicationCodes
                       {
                           CodeType = x.CodeType,
                           ApplicationCode = x.ApplicationCode,
                           CodeDesc = x.CodeDesc,
                           ShortCode = x.ShortCode,
                           CodeTypeControl = y.CodeTypeControl,
                           UsageStatus = x.UsageStatus,
                           DefaultStatus = x.DefaultStatus,
                           ActiveStatus = x.ActiveStatus,
                       }).OrderBy(o => o.ApplicationCode).ToListAsync();
                        return ds;
                    }
                    else
                    {
                        
                        var ds = await db.GtEcapcds
                       .Where(w => w.CodeType == codeType).Join(db.GtEcapcts.Where(c => c.CodeTypeControl == "U"),
                        x => x.CodeType,
                        y => y.CodeType,
                        (x, y) => new DO_ApplicationCodes
                        {
                            CodeType = x.CodeType,
                            ApplicationCode = x.ApplicationCode,
                            CodeDesc = x.CodeDesc,
                            ShortCode = x.ShortCode,
                            CodeTypeControl = y.CodeTypeControl,
                            UsageStatus = x.UsageStatus,
                            DefaultStatus = x.DefaultStatus,
                            ActiveStatus = x.ActiveStatus,
                        }).OrderBy(o => o.ApplicationCode).ToListAsync();
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoApplicationCodes(DO_ApplicationCodes obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_CodeDescExist = db.GtEcapcds.Where(w => w.CodeType == obj.CodeType
                                && w.CodeDesc.ToUpper().Replace(" ", "") == obj.CodeDesc.ToUpper().Replace(" ", "")).Count();
                        if (is_CodeDescExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0069", Message = string.Format(_localizer[name: "W0069"]) };
                        }

                        var is_DefaultStatusTrue = db.GtEcapcds.Where(w => w.DefaultStatus && w.CodeType == obj.CodeType && w.ActiveStatus).Count();
                        if (obj.DefaultStatus && is_DefaultStatusTrue > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0070", Message = string.Format(_localizer[name: "W0070"]) };
                        }

                        GtEcapct ap_ct = db.GtEcapcts.Where(w => w.CodeType == obj.CodeType).FirstOrDefault();
                        ap_ct.UsageStatus = true;
                        ap_ct.ModifiedBy = obj.UserID;
                        ap_ct.ModifiedOn = System.DateTime.Now;
                        ap_ct.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();

                        int maxAppcode = db.GtEcapcds.Where(w => w.CodeType == obj.CodeType).Select(c => c.ApplicationCode).DefaultIfEmpty().Max();
                        if (maxAppcode == 0)
                        {
                            maxAppcode = Convert.ToInt32(obj.CodeType.ToString() + "1".PadLeft(4, '0'));
                        }
                        else
                            maxAppcode = maxAppcode + 1;
                        if (!maxAppcode.ToString().StartsWith(obj.CodeType.ToString()))
                        {
                            
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0072", Message = string.Format(_localizer[name: "W0072"]) };
                        }

                        var ap_cd = new GtEcapcd
                        {
                            ApplicationCode = maxAppcode,
                            CodeType = obj.CodeType,
                            CodeDesc = obj.CodeDesc.Trim(),
                            ShortCode = obj.ShortCode,
                            DefaultStatus = obj.DefaultStatus,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID,

                        };
                        db.GtEcapcds.Add(ap_cd);

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateApplicationCodes(DO_ApplicationCodes obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcapcd ap_cd = db.GtEcapcds.Where(w => w.ApplicationCode == obj.ApplicationCode).FirstOrDefault();
                        if (ap_cd == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0071", Message = string.Format(_localizer[name: "W0071"]) };
                        }

                        IEnumerable<GtEcapcd> ls_apct = db.GtEcapcds.Where(w => w.CodeType == obj.CodeType).ToList();

                        var is_SameDescExists = ls_apct.Where(w => w.CodeDesc.ToUpper().Replace(" ", "") == obj.CodeDesc.ToUpper().Replace(" ", "")
                                && w.ApplicationCode != obj.ApplicationCode).Count();
                        if (is_SameDescExists > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0069", Message = string.Format(_localizer[name: "W0069"]) };
                        }

                        var is_DefaultStatusAssign = ls_apct.Where(w => w.DefaultStatus && w.CodeType == obj.CodeType
                                && w.ApplicationCode != obj.ApplicationCode && w.ActiveStatus).Count();
                        if (obj.DefaultStatus && is_DefaultStatusAssign > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0070", Message = string.Format(_localizer[name: "W0070"]) };
                        }

                        ap_cd.CodeDesc = obj.CodeDesc.Trim();
                        ap_cd.ShortCode = obj.ShortCode;
                        ap_cd.DefaultStatus = obj.DefaultStatus;
                        ap_cd.ActiveStatus = obj.ActiveStatus;
                        ap_cd.ModifiedBy = obj.UserID;
                        ap_cd.ModifiedOn = System.DateTime.Now;
                        ap_cd.ModifiedTerminal = obj.TerminalID;

                        await db.SaveChangesAsync();

                        List<GtEcapcd> ls_apcd = db.GtEcapcds.Where(w => w.CodeType == obj.CodeType).ToList();
                        bool isActive = false;
                        foreach (var f_apct in ls_apcd)
                        {
                            if (f_apct.ActiveStatus == true)
                            {
                                GtEcapct obj_CodeType = db.GtEcapcts.Where(w => w.CodeType == obj.CodeType).FirstOrDefault();
                                obj_CodeType.UsageStatus = true;

                                await db.SaveChangesAsync();
                                isActive = true;
                                break;
                            }
                        }

                        if (!isActive)
                        {
                            GtEcapct obj_CodeType = db.GtEcapcts.FirstOrDefault(x => x.CodeType == obj.CodeType);
                            obj_CodeType.UsageStatus = obj.ActiveStatus;

                            await db.SaveChangesAsync();
                        }

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveApplicationCode(bool status, int app_code)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcapcd ap_code = db.GtEcapcds.Where(w => w.ApplicationCode == app_code).FirstOrDefault();
                        if (ap_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0071", Message = string.Format(_localizer[name: "W0071"]) };
                        }

                        ap_code.ActiveStatus = status;
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
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

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
