import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { EmployeeService, EmployeeDto, DepartmentLookupDto } from '@proxy/employees';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrl: './employee.component.scss',
  providers: [ListService],
})
export class EmployeeComponent implements OnInit {
  employee = { items: [], totalCount: 0 } as PagedResultDto<EmployeeDto>;

  selectedEmployee = {} as EmployeeDto;

  departments$: Observable<DepartmentLookupDto[]>;

  form: FormGroup;

  isModalOpen = false;

  constructor(
    public readonly list: ListService, 
    private employeeService: EmployeeService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
    ) {
      this.departments$ = employeeService.getDepartmentLookup().pipe(map((r) => r.items));
    }
  
  ngOnInit() {
    const employeeStreamCreator = (query) => this.employeeService.getList(query);

    this.list.hookToQuery(employeeStreamCreator).subscribe((response) => {
      this.employee = response;
    });
  }

  createEmployee() {
    this.selectedEmployee = {} as EmployeeDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  // Add editEmployee method
  editBook(id: string) {
    this.employeeService.get(id).subscribe((employee) => {
      this.selectedEmployee = employee;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  // Add a delete method
  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.employeeService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  // add buildForm method
  buildForm() {
    this.form = this.fb.group({
      departmentId: [this.selectedEmployee.departmentId || null, Validators.required],
      name: [this.selectedEmployee.name || '', Validators.required],
      age: [this.selectedEmployee.age || null, Validators.required],
      salary: [this.selectedEmployee.salary || null, Validators.required],
    });
  }

  // add save method
  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedEmployee.id
      ? this.employeeService.update(this.selectedEmployee.id, this.form.value)
      : this.employeeService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}
