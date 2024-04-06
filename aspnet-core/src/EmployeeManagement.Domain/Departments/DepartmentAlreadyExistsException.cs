using Volo.Abp;

namespace EmployeeManagement.Departments;

public class DepartmentAlreadyExistsException : BusinessException
{
    public DepartmentAlreadyExistsException(string name)
        : base(EmployeeManagementDomainErrorCodes.DepartmentAlreadyExists)
    {
        WithData("name", name);
    }
}
