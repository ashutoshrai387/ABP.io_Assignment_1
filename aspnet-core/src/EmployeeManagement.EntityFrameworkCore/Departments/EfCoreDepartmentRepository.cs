using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using EmployeeManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EmployeeManagement.Departments;

public class EfCoreDepartmentRepository : EfCoreRepository<EmployeeManagementDbContext, Department, Guid>,
        IDepartmentRepository
{
    public EfCoreDepartmentRepository(
        IDbContextProvider<EmployeeManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<Department> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(department => department.Name == name);
    }

    public async Task<List<Department>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                department => department.Name.Contains(filter)
                )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }
}
