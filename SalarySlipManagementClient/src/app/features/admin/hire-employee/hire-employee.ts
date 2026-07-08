import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Employee } from '../../../core/services/employee';
import { Department } from '../../../core/services/department';
import { Role } from '../../../core/services/role';

@Component({
  selector: 'app-hire-employee',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './hire-employee.html',
  styleUrl: './hire-employee.css',
})
export class HireEmployee implements OnInit {
  hireForm: FormGroup;

  departments: any[] = [];
  roles: any[] = [];
  isLoading = false;
  successMessage = '';
  errorMessage = '';
  constructor(
    private fb: FormBuilder,
    private employeeService: Employee,
    private departmentService: Department,
    private roleService: Role,
    private cdr: ChangeDetectorRef,
  ) {
    this.hireForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.pattern(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&]{6,}$/,
          ),
        ],
      ],
      phoneNumber: [''],
      bankAccountNumber: [''],
      departmentId: ['', Validators.required],
      roleId: ['', Validators.required],
    });
  }

  preventAlphabets(event: KeyboardEvent) {
    const allowedChars = /[0-9\+\-\(\)\s]/;
    if (!allowedChars.test(event.key)) {
      event.preventDefault();
    }
  }

  ngOnInit() {
    if (typeof window === 'undefined') return;
    this.roleService.getAllRoles().subscribe({
      next: (data) => {
        this.roles = data;
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Failed to fetch roles', err),
    });

    this.departmentService.getAllDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Failed to fetch departments', err),
    });
  }

  onSubmit() {
    if (this.hireForm.invalid) {
      this.hireForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.employeeService.createNewEmployee(this.hireForm.value).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = 'Success! The employee has been hired and saved to SQL Server!';
        this.hireForm.reset();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage =
          err.error?.message || err.error || 'Failed to hire employee. Please try again.';
      },
    });
  }
}
