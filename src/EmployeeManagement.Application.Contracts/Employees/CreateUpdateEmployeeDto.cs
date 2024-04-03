using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Employees;

public class CreateUpdateEmployeeDto
{
    [Required]
    [StringLength(30)]
    public string Name { get; set; } = string.Empty;

    public Guid DepartmentId { get; set; }

    [Required]
    public int Age { get; set; }

    [Required]
    public float Salary { get; set; }
}
