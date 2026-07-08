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

  editingDepartmentId: string | null = null;
  editingRoleId: string | null = null;

  editingDepartmentName: string = '';
  editingRoleName: string = '';

  roleSuccess = '';
  roleError = '';
  departmentSuccess = '';
  departmentError = '';

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
    this.roleService.getAllRoles().subscribe({
      next: (data) => {
        this.roles = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err)
    });
    this.departmentService.getAllDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err)
    });
  }

  startEditDepartment(department: any) {
    this.editingDepartmentId = department.globalId;
    this.editingDepartmentName = department.departmentName;
  }

  saveEditDepartment() {
    if (!this.editingDepartmentId || !this.editingDepartmentName) return;

    this.departmentService
      .updateDepartment(this.editingDepartmentId, this.editingDepartmentName)
      .subscribe({
        next: () => {
          this.departmentSuccess = 'Department updated successfully!';
          
          const dept = this.departments.find(d => d.globalId === this.editingDepartmentId);
          if (dept) dept.departmentName = this.editingDepartmentName;

          this.editingDepartmentId = null;
          this.editingDepartmentName = '';
          this.cdr.markForCheck();
          
          setTimeout(() => (this.departmentSuccess = ''), 3000);
        },
        error: () => (this.departmentError = 'Failed to update department'),
      });
  }

  deleteDepartment(globalId: string) {
    if (confirm('Are you sure you want to delete this department?')) {
      this.departmentService.deleteDepartment(globalId).subscribe({
        next: () => {
          this.departmentSuccess = 'Department deleted successfully!';
          this.departments = this.departments.filter(d => d.globalId !== globalId);
          this.cdr.markForCheck();
          setTimeout(() => (this.departmentSuccess = ''), 3000);
        },
        error: () => (this.departmentError = 'Failed to delete department'),
      });
    }
  }

  startEditRole(role: any) {
    this.editingRoleId = role.globalId;
    this.editingRoleName = role.roleName;
  }

  saveEditRole() {
    if (!this.editingRoleId || !this.editingRoleName) return;

    this.roleService.updateRole(this.editingRoleId, this.editingRoleName).subscribe({
      next: () => {
        this.roleSuccess = 'Role updated successfully!';
        
        const r = this.roles.find(r => r.globalId === this.editingRoleId);
        if (r) r.roleName = this.editingRoleName;

        this.editingRoleId = null;
        this.editingRoleName = '';
        this.cdr.markForCheck();
        
        setTimeout(() => (this.roleSuccess = ''), 3000);
      },
      error: () => (this.roleError = 'Failed to update role'),
    });
  }

  deleteRole(globalId: string) {
    if (confirm('Are you sure you want to delete this role?')) {
      this.roleService.deleteRole(globalId).subscribe({
        next: () => {
          this.roleSuccess = 'Role deleted successfully!';
          this.roles = this.roles.filter(r => r.globalId !== globalId);
          this.cdr.markForCheck();
          setTimeout(() => (this.roleSuccess = ''), 3000);
        },
        error: () => (this.roleError = 'Failed to delete role'),
      });
    }
  }

  addRole() {
    if (this.roleForm.invalid) return;

    this.roleService.createNewRole(this.roleForm.value.name).subscribe({
      next: (res) => {
        this.roles.push({ roleName: this.roleForm.value.name, globalId: res.globalId }); // Add the returned ID for the new role
        this.roleSuccess = 'Role added successfully';
        this.roleForm.reset();
        // this.loadData();
        this.cdr.detectChanges();
        setTimeout(() => (this.roleSuccess = ''), 3000);
      },
      error: () => (this.roleError = 'Failed to add role'),
    });
  }

  addDepartment() {
    if (this.departmentForm.invalid) return;
    this.departmentService.createNewDepartment(this.departmentForm.value.name).subscribe({
      next: (res) => {
        this.departments.push({
          departmentName: this.departmentForm.value.name,
          globalId: res.globalId,
        }); // Add the returned ID for the new department
        this.departmentSuccess = 'Department added successfully';
        this.departmentForm.reset();
        // this.loadData();
        this.cdr.detectChanges();
        setTimeout(() => (this.departmentSuccess = ''), 3000);
      },
      error: () => (this.departmentError = 'Failed to add department'),
    });
  }
}
