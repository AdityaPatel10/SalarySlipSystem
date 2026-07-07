import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Employee } from '../../../core/services/employee';
import { SalarySlip } from '../../../core/services/salary-slip';
import { SalaryStructure } from '../../../core/services/salary-structure';
import { CommonModule } from '@angular/common';
import { PdfGenerator } from '../../../core/services/pdf-generator';

@Component({
  selector: 'app-employee-payroll',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './employee-payroll.html',
  styleUrl: './employee-payroll.css',
})
export class EmployeePayroll implements OnInit {
  employeeGlobalId: string = '';
  employeeData: any = null;

  structureForm!: FormGroup;
  hasStructure: boolean = false;
  currentStructureId: string | null = null;
  isSavingStructure: boolean = false;

  slipForm!: FormGroup;
  isGenerating = false;
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

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private structureService: SalaryStructure,
    private slipService: SalarySlip,
    private employeeService: Employee,
    private pdfService: PdfGenerator,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.employeeGlobalId = this.route.snapshot.paramMap.get('id') || '';

    this.structureForm = this.fb.group({
      basicSalary: ['', [Validators.required, Validators.min(0)]],
      hraPercentage: [20, Validators.required],
      otherAllowancesPercentage: [10, [Validators.required]],
      pfDeductionPercentage: [12, [Validators.required]],
      taxDeductionPercentage: [10, [Validators.required]],
    });

    this.slipForm = this.fb.group({
      month: [new Date().getMonth() + 1, Validators.required],
      year: [new Date().getFullYear(), Validators.required],
    });

    this.loadEmployeeData();
    this.loadSalaryStructure();
    this.loadSlipHistory();
  }

  loadEmployeeData() {
    this.employeeService.getEmployeeById(this.employeeGlobalId).subscribe((res) => {
      this.employeeData = res;
      this.cdr.detectChanges();
    });
  }

  loadSalaryStructure() {
    this.structureService.getAllSalaryStructures().subscribe((res) => {
      const struct = res.find((s) => s.employeeGlobalId === this.employeeGlobalId);
      if (struct) {
        this.hasStructure = true;
        this.currentStructureId = struct.globalId;
        this.structureForm.patchValue({
          basicSalary: struct.basicSalary,
          hraPercentage: struct.hraPercentage,
          otherAllowancesPercentage: struct.otherAllowancesPercentage,
          pfDeductionPercentage: struct.pfDeductionPercentage,
          taxDeductionPercentage: struct.taxDeductionPercentage,
        });
        this.cdr.detectChanges();
      }
    });
  }

  loadSlipHistory() {
    this.isLoadingHistory = true;

    this.slipService.getEmployeeHistory(this.employeeGlobalId).subscribe({
      next: (data) => {
        this.slipHistory = data;
        this.isLoadingHistory = false;
        this.cdr.detectChanges();
      },
      error: () => (this.isLoadingHistory = false),
    });
  }

  saveStructure() {
    if (this.structureForm.invalid) return;
    this.isSavingStructure = true;

    const payload = {
      employeeGlobalId: this.employeeGlobalId,
      ...this.structureForm.value,
    };

    if (this.hasStructure && this.currentStructureId) {
      this.structureService.updateSalaryStructure(this.currentStructureId, payload).subscribe({
        next: () => {
          alert('Salary structure updated successfully.');
          this.isSavingStructure = false;
        },
        error: () => (this.isSavingStructure = false),
      });
    } else {
      this.structureService.createSalaryStructure(payload).subscribe({
        next: (res) => {
          alert('Salary structure created successfully.');
          this.hasStructure = true;
          this.currentStructureId = res.globalId;
          this.isSavingStructure = false;
          this.cdr.detectChanges();
        },
        error: () => (this.isSavingStructure = false),
      });
    }
  }

  generateSlip() {
    if (this.slipForm.invalid) return;
    this.isGenerating = true;

    const payload = {
      employeeGlobalId: this.employeeGlobalId,
      month: Number(this.slipForm.value.month),
      year: Number(this.slipForm.value.year),
    };

    this.slipService.generateSalarySlip(payload).subscribe({
      next: (res) => {
        alert('PaySlip generated successfully.');
        this.isGenerating = false;
        this.loadSlipHistory();
        this.cdr.detectChanges();
      },
      error: (err) => {
        alert('Failed' + (err.error || 'Unknown Error'));
        this.isGenerating = false;
      },
    });
  }

  downloadPdf(slip: any) {
    this.pdfService.generatePaySlip(slip, this.employeeData);
  }
}
