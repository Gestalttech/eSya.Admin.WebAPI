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
    public class DepartmentRepository: IDepartmentRepository
    {
        private readonly IStringLocalizer<DepartmentRepository> _localizer;
        public DepartmentRepository(IStringLocalizer<DepartmentRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Department
        public async Task<List<DO_ApplicationCodes>> GetDepartmentCategoriesbyCodeType(int codetype)
        {
            using (var db = new eSyaEnterprise())
            {
                var ds =await db.GtEcapcds.Where(x => x.CodeType == codetype && x.ActiveStatus)
               .Select(s => new DO_ApplicationCodes
               {
                   ApplicationCode = s.ApplicationCode,
                   CodeDesc = s.CodeDesc

               }).OrderBy(x=>x.CodeDesc).ToListAsync();

                return ds;
            }

        }
        public async Task<List<DO_Department>> GetDepartmentsbycategoryId(int categoryId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (categoryId == 0)
                    {

                        var ds = db.GtEcdepts
                        .Select(a => new DO_Department
                        {
                            DeptCategory = a.DeptCategory,
                            DeptId=a.DeptId,
                            DeptDesc = a.DeptDesc,
                            ActiveStatus = a.ActiveStatus
                        }).OrderBy(x => x.DeptDesc).ToListAsync();

                        return await ds;
                    }
                    else
                    {
                        var ds = db.GtEcdepts.Where(x => x.DeptCategory == categoryId)
                        .Select(a => new DO_Department
                        {
                            DeptCategory = a.DeptCategory,
                            DeptId = a.DeptId,
                            DeptDesc = a.DeptDesc,
                            ActiveStatus = a.ActiveStatus
                        }).OrderBy(x => x.DeptDesc).ToListAsync();

                        return await ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
        public async Task<DO_ReturnParameter> InsertIntoDepartment(DO_Department obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var deptDescExist = db.GtEcdepts.Where(w => w.DeptCategory == obj.DeptCategory
                                && w.DeptDesc.ToUpper().Replace(" ", "") == obj.DeptDesc.ToUpper().Replace(" ", "")).Count();
                        if (deptDescExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0116", Message = string.Format(_localizer[name: "W0116"]) };
                        }

                        int maxId = db.GtEcdepts.Select(a => a.DeptId).DefaultIfEmpty().Max();
                        int dept_Id = maxId + 1;
                       
                        var dept = new GtEcdept
                        {
                            DeptCategory=obj.DeptCategory,
                            DeptId= dept_Id,
                            DeptDesc = obj.DeptDesc,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID,
                        };
                        db.GtEcdepts.Add(dept);

                        await db.SaveChangesAsync();

                        var _store = new GtEcstrm
                        {
                            StoreCode = dept_Id,
                            StoreType = 1,
                            StoreDesc = obj.DeptDesc,
                            FormId = obj.FormID,
                            ActiveStatus = obj.ActiveStatus,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcstrms.Add(_store);
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

        public async Task<DO_ReturnParameter> UpdateDepartment(DO_Department obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        IEnumerable<GtEcdept> lst_dept = db.GtEcdepts.Where(w => w.DeptCategory == obj.DeptCategory).ToList();

                        var is_SameDescExists = lst_dept.Where(w => w.DeptDesc.ToUpper().Replace(" ", "") == obj.DeptDesc.ToUpper().Replace(" ", "")
                                && w.DeptId != obj.DeptId).Count();
                        if (is_SameDescExists > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0116", Message = string.Format(_localizer[name: "W0116"]) };
                        }

                        GtEcdept dept = db.GtEcdepts.Where(x => x.DeptId == obj.DeptId && x.DeptCategory==obj.DeptCategory).FirstOrDefault();
                        if (dept == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0117", Message = string.Format(_localizer[name: "W0117"]) };
                        }

                        dept.DeptDesc = obj.DeptDesc;
                        dept.ModifiedBy = obj.UserID;
                        dept.ModifiedOn = System.DateTime.Now;
                        dept.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();


                        var store = db.GtEcstrms.Where(x => x.StoreCode == obj.DeptId && x.StoreType == 1).FirstOrDefault();
                        if (store != null)
                        {
                            store.StoreDesc = obj.DeptDesc;
                            store.ModifiedBy = obj.UserID;
                            store.ModifiedOn = System.DateTime.Now;
                            store.ModifiedTerminal = obj.TerminalID;
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveDepartment(bool status,int categoryId, int deptId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcdept dept = db.GtEcdepts.Where(w => w.DeptId == deptId && w.DeptCategory== categoryId).FirstOrDefault();
                        if (dept == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0117", Message = string.Format(_localizer[name: "W0117"]) };
                        }
                        var store = db.GtEcstrms.Where(x => x.StoreCode == deptId && x.StoreType == 1).FirstOrDefault();
                        if (store != null)
                        {
                            store.ActiveStatus= status;
                            await db.SaveChangesAsync();
                        }
                        dept.ActiveStatus = status;
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
