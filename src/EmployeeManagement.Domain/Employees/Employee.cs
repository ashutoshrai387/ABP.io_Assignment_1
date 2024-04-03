using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace EmployeeManagement.Employees;

public class Employee : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }

    public Guid DepartmentId { get; set; }

    public int Age { get; set; }

    public float Salary { get; set; }
}

