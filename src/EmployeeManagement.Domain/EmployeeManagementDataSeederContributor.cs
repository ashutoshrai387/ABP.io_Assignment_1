using EmployeeManagement.Departments;
using EmployeeManagement.Employees;
using EmployeeManagement.Permissions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Users;

namespace EmployeeManagement;

internal class EmployeeManagementDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityUserManager _identityUserManager;
    private readonly IRepository<Employee, Guid> _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly DepartmentManager _departmentManager;
    private readonly IdentityRoleManager _identityRoleManager;
    private readonly IPermissionManager _permissionManager;

    public EmployeeManagementDataSeederContributor(
        IRepository<Employee, Guid> employeeRepository,
        IDepartmentRepository departmentRepository,
        DepartmentManager departmentManager,
        IdentityUserManager identityUserManager,
        IdentityRoleManager identityRoleManager,
        IPermissionManager permissionManager)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _departmentManager = departmentManager;
        _identityUserManager = identityUserManager;
        _identityRoleManager = identityRoleManager;
        _permissionManager = permissionManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        //Seed initial data for employee and department
        if (await _employeeRepository.GetCountAsync() > 0)
        {
            return;
        }

        var CSE = await _departmentRepository.InsertAsync(
        await _departmentManager.CreateAsync("CSE")
        );

        var IT = await _departmentRepository.InsertAsync(
            await _departmentManager.CreateAsync("IT")
        );

        await _employeeRepository.InsertAsync(
            new Employee
            {
                DepartmentId = CSE.Id,
                Name = "Ashutosh Rai",
                Age = 34,
                Salary = 19.84f
            },
            autoSave: true
        );

        await _employeeRepository.InsertAsync(
            new Employee
            {
                DepartmentId = IT.Id,
                Name = "Subham Gupta",
                Age = 43,
                Salary = 42.0f
            },
            autoSave: true
        );


        // Create HR role if it doesn't exist
        var hrRole = await _identityRoleManager.FindByNameAsync("HR");
        if (hrRole == null)
        {
            await _identityRoleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), "HR"));
        }



        // Create new user with HR role if it doesn't exist
        var user = await _identityUserManager.FindByEmailAsync("hremail@email.com");
        if (user == null)
        {
            user = new IdentityUser(Guid.NewGuid(), "HR", "hremail@email.com");
            await _identityUserManager.CreateAsync(user, "1q2w3E*");
            await _identityUserManager.AddToRoleAsync(user, "HR");
        }



        // Set permissions to the HR role
        await _permissionManager.SetForRoleAsync("HR", EmployeeManagementPermissions.Departments.Default, true);
        await _permissionManager.SetForRoleAsync("HR", EmployeeManagementPermissions.Departments.Create, true);
        await _permissionManager.SetForRoleAsync("HR", EmployeeManagementPermissions.Departments.Edit, true);
        await _permissionManager.SetForRoleAsync("HR", EmployeeManagementPermissions.Departments.Delete, true);

        await _permissionManager.SetForRoleAsync("HR", EmployeeManagementPermissions.Employees.Default, true);
        await _permissionManager.SetForRoleAsync("HR", EmployeeManagementPermissions.Employees.Create, true);
        await _permissionManager.SetForRoleAsync("HR", EmployeeManagementPermissions.Employees.Edit, true);
        await _permissionManager.SetForRoleAsync("HR", EmployeeManagementPermissions.Employees.Delete, true);
    }
}