import type { CreateUpdateEmployeeDto, DepartmentLookupDto, EmployeeDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  apiName = 'Default';
  

  create = (input: CreateUpdateEmployeeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EmployeeDto>({
      method: 'POST',
      url: '/api/app/employee',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/employee/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EmployeeDto>({
      method: 'GET',
      url: `/api/app/employee/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getDepartmentLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<DepartmentLookupDto>>({
      method: 'GET',
      url: '/api/app/employee/department-lookup',
    },
    { apiName: this.apiName,...config });
  

  getEmployeeByDepartment = (departmentId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EmployeeDto[]>({
      method: 'GET',
      url: `/api/app/employee/employee-by-department/${departmentId}`,
    },
    { apiName: this.apiName,...config });
  

  getEmployeeByName = (employeeName: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EmployeeDto[]>({
      method: 'GET',
      url: '/api/app/employee/employee-by-name',
      params: { employeeName },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<EmployeeDto>>({
      method: 'GET',
      url: '/api/app/employee',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateEmployeeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EmployeeDto>({
      method: 'PUT',
      url: `/api/app/employee/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
