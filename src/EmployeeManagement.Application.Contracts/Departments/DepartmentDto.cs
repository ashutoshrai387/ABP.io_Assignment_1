using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace EmployeeManagement.Departments;

public class DepartmentDto : EntityDto<Guid>
{
    public string Name { get; set; }
}
