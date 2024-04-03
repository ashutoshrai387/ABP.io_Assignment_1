using AutoMapper;
using EmployeeManagement.Departments;
using EmployeeManagement.Employees;

namespace EmployeeManagement;

public class EmployeeManagementApplicationAutoMapperProfile : Profile
{
    public EmployeeManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Employee, EmployeeDto>();
        CreateMap<CreateUpdateEmployeeDto, Employee>();
        CreateMap<Department, DepartmentDto>();
        CreateMap<Department, DepartmentLookupDto>();
    }
}
