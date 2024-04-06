using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace EmployeeManagement.Departments;

public class Department : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }

    private Department()
    {
        /* This constructor is for deserialization / ORM purpose */
    }

    internal Department(
        Guid id,
        string name)
        : base(id)
    {
        SetName(name);
    }

    internal Department ChangeName(string name)
    {
        SetName(name);
        return this;
    }

    private void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: DepartmentConsts.MaxNameLength
        );
    }
}
