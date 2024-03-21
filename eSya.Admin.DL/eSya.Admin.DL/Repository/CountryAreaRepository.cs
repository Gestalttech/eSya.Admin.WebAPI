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
    public class CountryAreaRepository: ICountryAreaRepository
    {
        private readonly IStringLocalizer<CountryAreaRepository> _localizer;
        public CountryAreaRepository(IStringLocalizer<CountryAreaRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Address Area Header
        public async Task<List<DO_States>> GetActiveStatesbyISDCode(int isdCode)
        {
            using (var db = new eSyaEnterprise())
            {
                var states = db.GtAddrsts.Where(x => x.Isdcode == isdCode && x.ActiveStatus == true)
               .Select(s => new DO_States
               {
                   StateCode = s.StateCode,
                   StateDesc = s.StateDesc

               })
               .ToListAsync();
                return await states;
            }

        }
        public async Task<List<DO_Cities>> GetActiveCitiesbyStateCode(int isdCode, int stateCode)
        {
            using (var db = new eSyaEnterprise())
            {
                var cities = db.GtAddrcts.Where(x => x.Isdcode == isdCode && x.StateCode == stateCode && x.ActiveStatus == true)
               .Select(r => new DO_Cities
               {
                   CityCode = r.CityCode,
                   CityDesc = r.CityDesc,
               })
               .ToListAsync();
                return await cities;
            }
        }
        public async Task<List<DO_AddressHeader>> GetAddressAreaHeader(int isdCode, int stateCode, int cityCode)
        {
            using (var db = new eSyaEnterprise())
            {
                var cities = db.GtAddrhds.Join
                    (db.GtAddrsts,
                    ah => new { ah.Isdcode, ah.StateCode },
                    s => new { s.Isdcode, s.StateCode },
                    (ah, s) => new { ah, s }).
                    Join(db.GtAddrcts,
                    ahh => new { ahh.ah.CityCode },
                    c => new { c.CityCode },
                    (ahh, c) => new { ahh, c })
                    .Where(x => x.ahh.ah.Isdcode == isdCode
                    && x.ahh.ah.StateCode == stateCode
                    && x.ahh.ah.CityCode == cityCode)
               .Select(r => new DO_AddressHeader
               {
                   Isdcode = r.ahh.ah.Isdcode,
                   StateCode = r.ahh.ah.StateCode,
                   CityCode = r.ahh.ah.CityCode,
                   Zipcode = r.ahh.ah.Zipcode,
                   Zipdesc = r.ahh.ah.Zipdesc,
                   CityDesc = r.c.CityDesc,
                   StateDesc = r.ahh.s.StateDesc,
                   ActiveStatus = r.ahh.ah.ActiveStatus
               })
               .ToListAsync();
                return await cities;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoZipArea(DO_AddressHeader obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _zipExists = db.GtAddrhds.Where(w => w.Isdcode == obj.Isdcode && w.CityCode == obj.CityCode && w.StateCode == obj.StateCode
                        && w.Zipcode.ToUpper().Replace(" ", "") == obj.Zipcode.ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (_zipExists == null)
                        {
                            var _zipheader = new GtAddrhd
                            {
                                Isdcode = obj.Isdcode,
                                StateCode = obj.StateCode,
                                CityCode = obj.CityCode,
                                Zipcode = obj.Zipcode,
                                Zipdesc = obj.Zipdesc,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtAddrhds.Add(_zipheader);
                            await db.SaveChangesAsync();

                            if (obj._lstAddressdetails != null)
                            {
                                foreach (var ad in obj._lstAddressdetails)
                                {
                                    var _zdetails = await db.GtAddrdts.Where(w => w.Isdcode == obj.Isdcode && w.Zipcode.ToUpper().Replace(" ", "") == obj.Zipcode.ToUpper().Replace(" ", "") && w.ZipserialNumber == ad.ZipserialNumber).FirstOrDefaultAsync();
                                    if (_zdetails == null)
                                    {
                                        int max_serialNo = db.GtAddrdts.Where(y => y.Zipcode.ToUpper().Replace(" ", "") == obj.Zipcode.ToUpper().Replace(" ", "")).Select(c => c.ZipserialNumber).DefaultIfEmpty().Max();
                                        int _SerialNo = max_serialNo + 1;

                                        var _zd = new GtAddrdt
                                        {
                                            Isdcode = obj.Isdcode,
                                            Zipcode = obj.Zipcode,
                                            ZipserialNumber = _SerialNo,
                                            Area = ad.Area,
                                            ActiveStatus = ad.ActiveStatus,
                                            FormId = obj.FormID,
                                            CreatedBy = obj.UserID,
                                            CreatedOn = System.DateTime.Now,
                                            CreatedTerminal = obj.TerminalID
                                        };
                                        db.GtAddrdts.Add(_zd);
                                        await db.SaveChangesAsync();

                                    }
                                    else
                                    {
                                        _zdetails.Area = ad.Area;
                                        _zdetails.ActiveStatus = ad.ActiveStatus;
                                        _zdetails.FormId = obj.FormID;
                                        _zdetails.ModifiedBy = obj.UserID;
                                        _zdetails.ModifiedOn = System.DateTime.Now;
                                        _zdetails.ModifiedTerminal = obj.TerminalID;
                                    }
                                    await db.SaveChangesAsync();
                                }

                            }
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0074", Message = string.Format(_localizer[name: "W0074"]) };
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

        public async Task<DO_ReturnParameter> UpdateIntoZipArea(DO_AddressHeader obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _zipExists = db.GtAddrhds.Where(w => w.Isdcode == obj.Isdcode && w.CityCode == obj.CityCode && w.StateCode == obj.StateCode
                        && w.Zipcode.ToUpper().Replace(" ", "") == obj.Zipcode.ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (_zipExists != null)
                        {
                            _zipExists.Zipdesc = obj.Zipdesc;
                            _zipExists.ActiveStatus = obj.ActiveStatus;
                            _zipExists.FormId = obj.FormID;
                            _zipExists.ModifiedBy = obj.UserID;
                            _zipExists.ModifiedOn = System.DateTime.Now;
                            _zipExists.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();

                            if (obj._lstAddressdetails != null)
                            {
                                foreach (var ad in obj._lstAddressdetails)
                                {
                                    var _zdetails = await db.GtAddrdts.Where(w => w.Isdcode == obj.Isdcode && w.Zipcode.ToUpper().Replace(" ", "") == obj.Zipcode.ToUpper().Replace(" ", "") && w.ZipserialNumber == ad.ZipserialNumber).FirstOrDefaultAsync();
                                    if (_zdetails == null)
                                    {
                                        int max_serialNo = db.GtAddrdts.Where(y => y.Zipcode.ToUpper().Replace(" ", "") == obj.Zipcode.ToUpper().Replace(" ", "")).Select(c => c.ZipserialNumber).DefaultIfEmpty().Max();
                                        int _SerialNo = max_serialNo + 1;

                                        var _zd = new GtAddrdt
                                        {
                                            Isdcode = obj.Isdcode,
                                            Zipcode = obj.Zipcode,
                                            ZipserialNumber = _SerialNo,
                                            Area = ad.Area,
                                            ActiveStatus = ad.ActiveStatus,
                                            FormId = obj.FormID,
                                            CreatedBy = obj.UserID,
                                            CreatedOn = System.DateTime.Now,
                                            CreatedTerminal = obj.TerminalID
                                        };
                                        db.GtAddrdts.Add(_zd);
                                        await db.SaveChangesAsync();

                                    }
                                    else
                                    {
                                        _zdetails.Area = ad.Area;
                                        _zdetails.ActiveStatus = ad.ActiveStatus;
                                        _zdetails.FormId = obj.FormID;
                                        _zdetails.ModifiedBy = obj.UserID;
                                        _zdetails.ModifiedOn = System.DateTime.Now;
                                        _zdetails.ModifiedTerminal = obj.TerminalID;
                                    }
                                    await db.SaveChangesAsync();
                                }

                            }
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0075", Message = string.Format(_localizer[name: "W0075"]) };
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

        #region Address Area Details
        public async Task<List<DO_AddressDetails>> GetAddressAreaDetails(int isdCode, int stateCode, int cityCode, string zipCode)
        {
            using (var db = new eSyaEnterprise())
            {
                var cities = db.GtAddrdts.
                    Join(db.GtAddrhds,
                    ad => new { ad.Zipcode },
                    zh => new { zh.Zipcode },
                    (ad, zh) => new { ad, zh })
                   .Join(db.GtAddrsts,
                    ah => new { ah.zh.Isdcode, ah.zh.StateCode },
                    s => new { s.Isdcode, s.StateCode },
                    (ah, s) => new { ah, s }).
                    Join(db.GtAddrcts,
                    ahh => new { ahh.ah.zh.CityCode },
                    c => new { c.CityCode },
                    (ahh, c) => new { ahh, c })
                    .Where(x => x.ahh.ah.zh.Isdcode == isdCode
                    && x.ahh.ah.zh.StateCode == stateCode
                    && x.ahh.ah.zh.CityCode == cityCode
                    && x.ahh.ah.ad.Zipcode == zipCode)
               .Select(r => new DO_AddressDetails
               {
                   Isdcode = r.ahh.ah.ad.Isdcode,
                   Zipcode = r.ahh.ah.ad.Zipcode,
                   ZipserialNumber = r.ahh.ah.ad.ZipserialNumber,
                   Area = r.ahh.ah.ad.Area,
                   ActiveStatus = r.ahh.ah.ad.ActiveStatus
               })
               .ToListAsync();
                return await cities;
            }
        }
        #endregion

        
    }
}
