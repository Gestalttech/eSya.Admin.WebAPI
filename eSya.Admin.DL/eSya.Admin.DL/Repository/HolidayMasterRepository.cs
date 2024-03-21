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
    public class HolidayMasterRepository: IHolidayMasterRepository
    {
        private readonly IStringLocalizer<HolidayMasterRepository> _localizer;
        public HolidayMasterRepository(IStringLocalizer<HolidayMasterRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Holiday Master
        public async Task<List<DO_HolidayMaster>> GetHolidayByBusinessKey(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEchlms
                    .Where(w => w.BusinessKey == BusinessKey).Select(r => new DO_HolidayMaster
                    {
                        BusinessKey = r.BusinessKey,
                        Year=r.Year,
                        HolidayType=r.HolidayType,
                        HolidayDate = r.HolidayDate,
                        HolidayDesc = r.HolidayDesc,
                        ActiveStatus = r.ActiveStatus

                    }).ToListAsync();

                    return await ds;
                }

            }

            catch (Exception ex)
            {

                throw ex;

            }
        }

        public async Task<DO_ReturnParameter> InsertIntoHolidayMaster(DO_HolidayMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _IsHolidayExits = db.GtEchlms.Where(w => w.BusinessKey == obj.BusinessKey && w.Year==obj.Year && w.HolidayDate == obj.HolidayDate && w.HolidayType.ToUpper().Replace(" ","")==obj.HolidayType.ToUpper().Replace(" ","")).FirstOrDefault();
                        if (_IsHolidayExits != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0132", Message = string.Format(_localizer[name: "W0132"]) };
                        }
                        else
                        {
                            var hp = new GtEchlm 
                            {
                                BusinessKey = obj.BusinessKey,
                                Year=obj.Year,
                                HolidayType= obj.HolidayType,
                                HolidayDate = obj.HolidayDate,
                                HolidayDesc = obj.HolidayDesc,
                                ActiveStatus = obj.ActiveStatus,
                                FormId=obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEchlms.Add(hp);
                            await db.SaveChangesAsync();
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };


                        }
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

        public async Task<DO_ReturnParameter> UpdateHolidayMaster(DO_HolidayMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _HM = db.GtEchlms.Where(w => w.BusinessKey == obj.BusinessKey && w.Year == obj.Year && w.HolidayDate == obj.HolidayDate && w.HolidayType.ToUpper().Replace(" ", "") == obj.HolidayType.ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (_HM == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0133", Message = string.Format(_localizer[name: "W0133"]) };
                        }
                        else
                        {
                            _HM.HolidayDesc = obj.HolidayDesc;
                            _HM.ActiveStatus = obj.ActiveStatus;
                            _HM.ModifiedBy = obj.UserID;
                            _HM.ModifiedOn = System.DateTime.Now;
                            _HM.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }


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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveHolidayMaster(DO_HolidayMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _HM = db.GtEchlms.Where(w => w.BusinessKey == obj.BusinessKey && w.Year == obj.Year && w.HolidayDate == obj.HolidayDate && w.HolidayType.ToUpper().Replace(" ", "") == obj.HolidayType.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (_HM == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0133", Message = string.Format(_localizer[name: "W0133"]) };
                        }
                        else
                        {

                            _HM.ActiveStatus = obj._status;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            if (obj._status == true)

                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                            else
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                        }


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
