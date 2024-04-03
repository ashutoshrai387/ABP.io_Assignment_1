using EmployeeManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EmployeeManagement.Permissions;

public class EmployeeManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        //var myGroup = context.AddGroup(EmployeeManagementPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(EmployeeManagementPermissions.MyPermission1, L("Permission:MyPermission1"));

        var employeeManagementGroup = context.AddGroup(EmployeeManagementPermissions.GroupName, L("Permission:EmployeeManagement"));

        var employeesPermission = employeeManagementGroup.AddPermission(EmployeeManagementPermissions.Employees.Default, L("Permission:Employees"));
        employeesPermission.AddChild(EmployeeManagementPermissions.Employees.Create, L("Permission:Employees.Create"));
        employeesPermission.AddChild(EmployeeManagementPermissions.Employees.Edit, L("Permission:Employees.Edit"));
        employeesPermission.AddChild(EmployeeManagementPermissions.Employees.Delete, L("Permission:Employees.Delete"));

        var departmentsPermission = employeeManagementGroup.AddPermission(EmployeeManagementPermissions.Departments.Default, L("Permission:Departments"));
        departmentsPermission.AddChild(EmployeeManagementPermissions.Departments.Create, L("Permission:Departments.Create"));
        departmentsPermission.AddChild(EmployeeManagementPermissions.Departments.Edit, L("Permission:Departments.Edit"));
        departmentsPermission.AddChild(EmployeeManagementPermissions.Departments.Delete, L("Permission:Departments.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EmployeeManagementResource>(name);
    }
}