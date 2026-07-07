import { Injectable } from '@angular/core';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Injectable({
  providedIn: 'root',
})
export class PdfGenerator {
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

  generatePaySlip(slip: any, employeeData: any) {
    if (!employeeData || !slip) return;
    const doc = new jsPDF();

    doc.setFontSize(22);
    doc.setTextColor(41, 128, 185);
    doc.text('Salary Slip', 105, 20, { align: 'center' });

    doc.setFontSize(11);
    doc.setTextColor(100);
    doc.text(
      `Salary Slip for: ${this.months.find((m) => m.value === slip.month)?.name} ${slip.year}`,
      14,
      30,
    );

    doc.text(`Generated On: ${new Date().toLocaleDateString()}`, 14, 36);

    autoTable(doc, {
      startY: 45,
      head: [['Employee Details', '']],
      body: [
        ['Name:', employeeData.name],
        ['Email:', employeeData.email],
        ['Phone:', employeeData.phoneNumber || 'N/A'],
        ['Bank Account:', employeeData.bankAccountNumber || 'N/A'],
        ['Department:', employeeData.departmentName || 'N/A'],
        ['Role:', employeeData.roleName || 'N/A'],
      ],
      theme: 'grid',
      headStyles: { fillColor: [41, 128, 185] },
    });

    const basic = slip.basicSalary ?? 0;
    const hra = slip.hra ?? 0;
    const other = slip.otherAllowances ?? 0;
    const pf = slip.pfDeduction ?? 0;
    const tax = slip.taxDeduction ?? 0;

    const gross = slip.grossSalary ?? 0;
    const deductions = slip.totalDeduction ?? 0;
    const net = slip.netSalary ?? 0;

    autoTable(doc, {
      startY: (doc as any).lastAutoTable.finalY + 10,
      head: [['Earning & Deductions', 'Amount ($)']],
      body: [
        ['Basic Salary:', `$${basic.toFixed(2)}`],
        ['HRA:', `$${hra.toFixed(2)}`],
        ['Other Allowances:', `$${other.toFixed(2)}`],
        ['Pf Deduction:', `$${pf.toFixed(2)}`],
        ['Tax Deduction:', `$${tax.toFixed(2)}`],
        ['Gross Salary:', `$${gross.toFixed(2)}`],
        ['Total Deductions:', `$${deductions.toFixed(2)}`],
        ['Net Salary:', `$${net.toFixed(2)}`],
      ],
      theme: 'striped',
      headStyles: { fillColor: [46, 204, 113] },
      foot: [['NET PAY', `$${net.toFixed(2)}`]],
      footStyles: { fillColor: [41, 128, 185], fontSize: 14 },
    });

    doc.save(`${employeeData.name.replace(/\s+/g, '_')}_Payslip_${slip.month}_${slip.year}.pdf`);
  }
}
