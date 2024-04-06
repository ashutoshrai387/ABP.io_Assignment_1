import { Component } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { DepartmentService, DepartmentDto } from '@proxy/departments';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-department',
  templateUrl: './department.component.html',
  styleUrl: './department.component.scss',
  providers: [ListService],
})
export class DepartmentComponent {
  department = { items: [], totalCount: 0 } as PagedResultDto<DepartmentDto>;

  isModalOpen = false;

  form: FormGroup;

  selectedDepartment = {} as DepartmentDto;

  constructor(
    public readonly list: ListService,
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit(): void {
    const departmentStreamCreator = (query) => this.departmentService.getList(query);

    this.list.hookToQuery(departmentStreamCreator).subscribe((response) => {
      this.department = response;
    });
  }

  createDepartment() {
    this.selectedDepartment = {} as DepartmentDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editDepartment(id: string) {
    this.departmentService.get(id).subscribe((department) => {
      this.selectedDepartment = department;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedDepartment.name || '', Validators.required]
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    if (this.selectedDepartment.id) {
      this.departmentService
        .update(this.selectedDepartment.id, this.form.value)
        .subscribe(() => {
          this.isModalOpen = false;
          this.form.reset();
          this.list.get();
        });
    } else {
      this.departmentService.create(this.form.value).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
      });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure')
        .subscribe((status) => {
          if (status === Confirmation.Status.confirm) {
            this.departmentService.delete(id).subscribe(() => this.list.get());
          }
	    });
  }
}
