import type { AuditedEntityDto, EntityDto } from '@abp/ng.core';

export interface CreateUpdateEmployeeDto {
  name: string;
  departmentId?: string;
  age: number;
  salary: number;
}

export interface DepartmentLookupDto extends EntityDto<string> {
  name?: string;
}

export interface EmployeeDto extends AuditedEntityDto<string> {
  name?: string;
  departmentId?: string;
  departmentName?: string;
  age: number;
  salary: number;
}
