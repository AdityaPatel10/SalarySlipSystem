import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Employee } from '../../../core/services/employee';
import { Department } from '../../../core/services/department';
import { EditEmployeeModal } from '../../../shared/components/edit-employee-modal/edit-employee-modal';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, EditEmployeeModal, RouterModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  stats = {
    totalEmployees: 0,
    totalDepartments: 0,
    monthlyPayroll: '$0',
  };

  recentEmployees: any[] = [];
  isLoading = true;

  isEditModalOpen = false;
  selectedEmployeeForEdit: any = null;

  constructor(
    private departmentService: Department,
    private employeeService: Employee,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    if (typeof window === 'undefined') return;
    this.employeeService.getAllEmployees().subscribe({
      next: (data) => {
        this.recentEmployees = data;
        this.stats.totalEmployees = data.length;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to fetch employees', err);
        this.isLoading = false;
      },
    });
    this.departmentService.getAllDepartments().subscribe({
      next: (data) => {
        this.stats.totalDepartments = data.length;
        this.cdr.detectChanges();
      },
    });
  }

  fireEmployee(globalId: string) {
    if (confirm('Are you sure you want to fire this employee? This action cannot be undone.')) {
      this.employeeService.deleteEmployee(globalId).subscribe({
        next: () => {
          alert('Employee fired successfully.');
          this.ngOnInit(); // Refresh the list of employees
        },
        error: (err) => {
          console.error('Failed to fire employee', err);
          alert('Failed to fire employee. Please try again later.');
        },
      });
    }
  }

  openEditModal(emp: any) {
    this.selectedEmployeeForEdit = emp;
    this.isEditModalOpen = true;
  }
}
