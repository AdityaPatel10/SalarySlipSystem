import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Employee } from '../../../core/services/employee';
import { SalarySlip } from '../../../core/services/salary-slip';
import { Router } from '@angular/router';
import { PdfGenerator } from '../../../core/services/pdf-generator';
import { ProfileCard } from '../../../shared/components/profile-card/profile-card';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-employee-dashboard',
  imports: [CommonModule, ProfileCard, ReactiveFormsModule],
  templateUrl: './employee-dashboard.html',
  styleUrl: './employee-dashboard.css',
})
export class EmployeeDashboard implements OnInit {
  globalId: string | null = null;
  employeeData: any = null;

  activeTab: 'payslips' | 'settings' = 'payslips';

  months = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, name: 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' },
  ];

  slipHistory: any[] = [];
  isLoadingHistory = true;

  // Settings Form
  settingsForm: FormGroup;
  isSavingSettings = false;
  settingsSuccessMessage = '';
  settingsErrorMessage = '';

  constructor(
    private employeeService: Employee,
    private slipService: SalarySlip,
    private router: Router,
    private pdfService: PdfGenerator,
    private cdr: ChangeDetectorRef,
    private fb: FormBuilder,
  ) {
    this.settingsForm = this.fb.group({
      name: [''],
      email: [''],
      phoneNumber: [''],
      bankAccountNumber: [''],
      newPassword: [''],
    });
  }

  ngOnInit(): void {
    if (typeof window !== 'undefined') {
      this.globalId = localStorage.getItem('global_id');

      if (!this.globalId) {
        this.logout();
        return;
      }

      this.loadEmployeeData();
      this.loadSlipHistory();
    }
  }

  loadEmployeeData() {
    this.employeeService.getEmployeeById(this.globalId!).subscribe({
      next: (res) => {
        this.employeeData = res;
        this.settingsForm.patchValue({
          name: res.name,
          email: res.email,
          phoneNumber: res.phoneNumber,
          bankAccountNumber: res.bankAccountNumber,
        });
        this.cdr.markForCheck();
      },
      error: () => this.logout(),
    });
  }

  loadSlipHistory() {
    this.isLoadingHistory = true;

    this.slipService.getEmployeeHistory(this.globalId!).subscribe({
      next: (data) => {
        this.slipHistory = data;
        this.isLoadingHistory = false;
        this.cdr.markForCheck();
      },
      error: () => {
        this.isLoadingHistory = false;
        this.cdr.markForCheck();
      },
    });
  }

  onUpdateSettings() {
    this.isSavingSettings = true;
    this.settingsErrorMessage = '';
    this.settingsSuccessMessage = '';

    this.employeeService.updateSettings(this.settingsForm.value).subscribe({
      next: () => {
        this.isSavingSettings = false;
        this.settingsSuccessMessage = 'Profile updated successfully!';
        this.settingsForm.patchValue({ newPassword: '' });
        this.loadEmployeeData();
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isSavingSettings = false;
        this.settingsErrorMessage = err.error?.message || 'Failed to update profile.';
        this.cdr.detectChanges();
      },
    });
  }

  logout() {
    if (typeof window !== 'undefined') {
      localStorage.clear();
    }
    this.router.navigate(['/']);
  }

  downloadPdf(slip: any) {
    this.pdfService.generatePaySlip(slip, this.employeeData);
  }
}
