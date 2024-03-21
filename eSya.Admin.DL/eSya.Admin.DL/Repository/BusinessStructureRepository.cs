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
    public class BusinessStructureRepository: IBusinessStructureRepository
    {
        private readonly IStringLocalizer<BusinessStructureRepository> _localizer;
        public BusinessStructureRepository(IStringLocalizer<BusinessStructureRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Business Statutory
        public async Task<List<DO_BusinessLocation>> GetBusinessKey()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcbslns
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessKey = r.BusinessKey,
                            LocationDescription = r.BusinessName + "-" + r.LocationDescription
                        }).ToListAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_BusinessStatutoryDetails> GetISDCodeByBusinessKey(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcbslns.Where(x => x.BusinessKey == BusinessKey && x.ActiveStatus).Select(r=>
                       new DO_BusinessStatutoryDetails
                       {
                           IsdCode = r.Isdcode,
                       }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_BusinessStatutoryDetails>> GetStatutoryInformation(int BusinessKey, int isdCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    //var ds = db.GtEccnsds
                    //    .Join(db.GtEcsupas.Where(w => w.ParameterId == 1),
                    //        x => new { x.Isdcode, x.StatutoryCode },
                    //    y => new { y.Isdcode, y.StatutoryCode },
                    //       (x, y) => new { x, y })
                    //    .GroupJoin(db.GtEcbssds.Where(w => w.BusinessKey == BusinessKey),
                    //     xy => xy.x.StatutoryCode,
                    //     c => c.StatutoryCode,
                    //     (xy, c) => new { xy, c = c.FirstOrDefault() }).DefaultIfEmpty()
                    //    .Where(w => w.xy.x.Isdcode == isdCode && (bool)w.xy.x.ActiveStatus)
                    //    .Select(r => new DO_BusinessStatutoryDetails
                    //    {
                    //        BusinessKey = BusinessKey,
                    //        StatutoryCode = r.xy.x.StatutoryCode,
                    //        StatutoryDescription = r.xy.x.StatutoryDescription,
                    //        StatutoryValue = r.c != null ? r.c.StatutoryDescription : "",
                    //        ActiveStatus = r.c != null ? r.c.ActiveStatus : r.xy.x.ActiveStatus
                    //    }).ToListAsync();

                    //return await ds;

                    var ds = db.GtEccnsds.Where(x=>x.Isdcode==isdCode && x.ActiveStatus)
                        .Join(db.GtEcsupas.Where(w => w.ParameterId == 1),
                            x => new { x.Isdcode, x.StatutoryCode },
                            y => new { y.Isdcode, y.StatutoryCode },
                           (x, y) => new { x, y })
                        .GroupJoin(db.GtEcbssds.Where(w => w.BusinessKey == BusinessKey),
                         xy => xy.x.StatutoryCode,
                         c => c.StatutoryCode,
                         (xy, c) => new { xy, c })
                        
                        .SelectMany(z=>z.c.DefaultIfEmpty(),
                        (a,b) => new DO_BusinessStatutoryDetails
                        {
                            BusinessKey = BusinessKey,
                            StatutoryCode =a.xy.x.StatutoryCode, 
                            StatutoryDescription =a.xy.x.StatutoryDescription, 
                            StatutoryValue =b!=null?b.StatutoryDescription:"", 
                            ActiveStatus =b!=null?b.ActiveStatus:a.xy.x.ActiveStatus 
                        }).ToListAsync();

                    return await ds;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateBusinessStatutory(List<DO_BusinessStatutoryDetails> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_StatutoryDetailEnter = obj.Where(w => !String.IsNullOrEmpty(w.StatutoryValue)).Count();
                        if (is_StatutoryDetailEnter <= 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0073", Message = string.Format(_localizer[name: "W0073"]) };
                        }

                        foreach (var sd in obj.Where(w => !String.IsNullOrEmpty(w.StatutoryValue)))
                        {
                            GtEcbssd cs_sd = db.GtEcbssds.Where(x => x.BusinessKey == sd.BusinessKey
                                            && x.StatutoryCode == sd.StatutoryCode).FirstOrDefault();
                            if (cs_sd == null)
                            {
                                var o_cssd = new GtEcbssd
                                {
                                    BusinessKey = sd.BusinessKey,
                                    StatutoryCode = sd.StatutoryCode,
                                    StatutoryDescription = sd.StatutoryValue,
                                    ActiveStatus = sd.ActiveStatus,
                                    FormId = sd.FormID,
                                    CreatedBy = sd.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = sd.TerminalID
                                };
                                db.GtEcbssds.Add(o_cssd);
                            }
                            else
                            {
                                cs_sd.StatutoryDescription = sd.StatutoryValue;
                                cs_sd.ActiveStatus = sd.ActiveStatus;
                                cs_sd.ModifiedBy = sd.UserID;
                                cs_sd.ModifiedOn = System.DateTime.Now;
                                cs_sd.ModifiedTerminal = sd.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }

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

        #endregion 
    }
}
