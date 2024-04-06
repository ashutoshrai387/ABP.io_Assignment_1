using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EmployeeManagement.Employees;

public interface IEmployeeAppService :
    ICrudAppService< 
        EmployeeDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateEmployeeDto> 
{
    Task<ListResultDto<DepartmentLookupDto>> GetDepartmentLookupAsync();
    Task<List<EmployeeDto>> GetEmployeeByDepartmentAsync(Guid departmentId);
    Task<List<EmployeeDto>> GetEmployeeByNameAsync(String employeeName);
}