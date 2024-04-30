using eSya.Admin.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Admin.IF
{
    public interface IDepartmentRepository
    {
        Task<List<DO_ApplicationCodes>> GetDepartmentCategoriesbyCodeType(int codetype);
        Task<List<DO_Department>> GetDepartmentsbycategoryId(int categoryId);
        Task<DO_ReturnParameter> InsertIntoDepartment(DO_Department obj);
        Task<DO_ReturnParameter> UpdateDepartment(DO_Department obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveDepartment(bool status, int categoryId, int deptId);
    }
}
