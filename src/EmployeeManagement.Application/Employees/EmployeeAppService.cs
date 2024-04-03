using EmployeeManagement.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using EmployeeManagement.Departments;
using Volo.Abp.ObjectMapping;

namespace EmployeeManagement.Employees;

[Authorize(EmployeeManagementPermissions.Employees.Default)]
public class EmployeeAppService :
    CrudAppService<
        Employee, 
        EmployeeDto,
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateEmployeeDto>, 
    IEmployeeAppService 
{
    private readonly IDepartmentRepository _departmentRepository;
    
    public EmployeeAppService(
        IRepository<Employee, Guid> repository, 
        IDepartmentRepository departmentRepository)
        : base(repository)
    {
        _departmentRepository = departmentRepository;
        GetPolicyName = EmployeeManagementPermissions.Employees.Default;
        GetListPolicyName = EmployeeManagementPermissions.Employees.Default;
        CreatePolicyName = EmployeeManagementPermissions.Employees.Create;
        UpdatePolicyName = EmployeeManagementPermissions.Employees.Edit;
        DeletePolicyName = EmployeeManagementPermissions.Employees.Delete;
    }

    //public IRepository<Employee, Guid> Get_repository()
    //{
    //    return repository;
    //}

    


    public override async Task<EmployeeDto> GetAsync(Guid id)
    {
        //Get the IQueryable<Book> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join employee and authors
        var query = from employee in queryable
                    join department in await _departmentRepository.GetQueryableAsync() on employee.DepartmentId equals department.Id
                    where employee.Id == id
                    select new { employee, department };

        //Execute the query and get the book with author
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Employee), id);
        }

        var employeeDto = ObjectMapper.Map<Employee, EmployeeDto>(queryResult.employee);
        employeeDto.DepartmentName = queryResult.department.Name;
        return employeeDto;
    }

    public override async Task<PagedResultDto<EmployeeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Book> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join books and authors
        var query = from employee in queryable
                    join department in await _departmentRepository.GetQueryableAsync() on employee.DepartmentId equals department.Id
                    select new { employee, department };

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of BookDto objects
        var employeeDtos = queryResult.Select(x =>
        {
            var employeeDto = ObjectMapper.Map<Employee, EmployeeDto>(x.employee);
            employeeDto.DepartmentName = x.department.Name;
            return employeeDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<EmployeeDto>(
            totalCount,
            employeeDtos
        );
    }

    public async Task<ListResultDto<DepartmentLookupDto>> GetDepartmentLookupAsync()
    {
        var departments = await _departmentRepository.GetListAsync();

        return new ListResultDto<DepartmentLookupDto>(
            ObjectMapper.Map<List<Department>, List<DepartmentLookupDto>>(departments)
        );
    }

    public async Task<List<EmployeeDto>> GetEmployeeByDepartmentAsync(Guid departmentId)
    {
        // Retrieve IQueryable<Employee> from the repository
        var queryable = await Repository.GetQueryableAsync();

        // Filter employees by department ID
        var employees = queryable.Where(e => e.DepartmentId == departmentId).ToList();

        // Map employees to DTOs
        return ObjectMapper.Map<List<Employee>, List<EmployeeDto>>(employees);
    }

    public async Task<List<EmployeeDto>> GetEmployeeByNameAsync(String employeeName)
    {
        // Retrieve IQueryable<Employee> from the repository
        var queryable = await Repository.GetQueryableAsync();

        // Filter employees by department ID
        var employees = queryable.Where(e => e.Name == employeeName).ToList();

        // Map employees to DTOs
        return ObjectMapper.Map<List<Employee>, List<EmployeeDto>>(employees);
    }



    private static string NormalizeSorting(string sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"employee.{nameof(Employee.Name)}";
        }

        if (sorting.Contains("departmentName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "departmentName",
                "department.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"employee.{sorting}";
    }
}
