import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Employee } from '../../../core/services/employee';
import { Department } from '../../../core/services/department';
import { Role } from '../../../core/services/role';

@Component({
  selector: 'app-edit-employee-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-employee-modal.html',
  styleUrl: './edit-employee-modal.css',
})
export class EditEmployeeModal implements OnInit {
  @Input() employeeData: any;
  @Output() close = new EventEmitter<void>();
  @Output() refresh = new EventEmitter<void>();

  editForm!: FormGroup;
  departments: any[] = [];
  roles: any[] = [];
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private employeeService: Employee,
    private departmentService: Department,
    private roleService: Role,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.departmentService.getAllDepartments().subscribe((d) => {
      this.departments = d;
      this.cdr.detectChanges();

    });
      
    this.roleService.getAllRoles().subscribe((r) => {
      this.roles = r; 
      this.cdr.detectChanges();
    });

    this.editForm = this.fb.group({
      name: [this.employeeData.name, Validators.required],
      email: [this.employeeData.email, [Validators.required, Validators.email]],
      phoneNumber: [this.employeeData.phoneNumber],
      bankAccountNumber: [this.employeeData.bankAccountNumber],
      departmentId: [this.employeeData.departmentGlobalId, Validators.required],
      roleId: [this.employeeData.roleGlobalId, Validators.required],
    });
  }

  submitEdit() {
    if (this.editForm.invalid) return;
    this.isLoading = true;

    this.employeeService.updateEmployee(this.employeeData.globalId, this.editForm.value).subscribe({
      next: () => {
        alert('Employee details updated successfully.');
        this.isLoading = false;
        this.refresh.emit();
        this.close.emit();
      },
      error: (err) => {
        console.error('Failed to update employee details', err);
        alert('Failed to update employee details. Please try again later.');
        this.isLoading = false;
      },
    });
  }
}
