﻿using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace EmployeeManagement.Departments;

public class DepartmentManager : DomainService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentManager(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<Department> CreateAsync(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var existingAuthor = await _departmentRepository.FindByNameAsync(name);
        if (existingAuthor != null)
        {
            throw new DepartmentAlreadyExistsException(name);
        }

        return new Department(GuidGenerator.Create(),name);
    }

    public async Task ChangeNameAsync(
        Department department,
        string newName)
    {
        Check.NotNull(department, nameof(department));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));

        var existingDepartment = await _departmentRepository.FindByNameAsync(newName);
        if (existingDepartment != null && existingDepartment.Id != department.Id)
        {
            throw new DepartmentAlreadyExistsException(newName);
        }

        department.ChangeName(newName);
    }
}
