import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Department } from '../../../core/services/department';
import { Role } from '../../../core/services/role';

@Component({
  selector: 'app-master-data',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './master-data.html',
  styleUrl: './master-data.css',
})
export class MasterData implements OnInit {
  [x: string]: any;
  roleForm: FormGroup;
  departmentForm: FormGroup;

  roles: any[] = [];
  departments: any[] = [];

  roleMessage = '';
  departmentMessage = '';

  constructor(
    private fb: FormBuilder,
    private roleService: Role,
    private departmentService: Department,
    private cdr: ChangeDetectorRef,
  ) {
    this.roleForm = this.fb.group({ name: ['', Validators.required] });
    this.departmentForm = this.fb.group({ name: ['', Validators.required] });
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.roleService.getAllRoles().subscribe((data) => {
      this.roles = data;
      this.cdr.detectChanges();
    });
    this.departmentService.getAllDepartments().subscribe((data) => {
      this.departments = data;
      this.cdr.detectChanges();
    });
  }

  addRole() {
    if (this.roleForm.invalid) return;

    this.roleService.createNewRole(this.roleForm.value.name).subscribe({
      next: () => {
        this.roleMessage = 'Role added successfully';
        this.roleForm.reset();
        this.loadData();
        setTimeout(() => (this.roleMessage = ''), 3000);
      },
      error: () => (this.roleMessage = 'Failed to add role'),
    });
  }

  addDepartment() {
    if (this.departmentForm.invalid) return;
    this.departmentService.createNewDepartment(this.departmentForm.value.name).subscribe({
      next: () => {
        this.departmentMessage = 'Department added successfully';
        this.departmentForm.reset();
        this.loadData();
        setTimeout(() => (this.departmentMessage = ''), 3000);
      },
      error: () => (this.departmentMessage = 'Failed to add department'),
    });
  }
}
