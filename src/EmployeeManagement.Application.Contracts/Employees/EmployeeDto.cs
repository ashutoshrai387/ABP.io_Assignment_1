using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace EmployeeManagement.Employees;

public class EmployeeDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public Guid DepartmentId { get; set; }

    public string DepartmentName { get; set; }

    public int Age { get; set; }

    public float Salary { get; set; }
}
