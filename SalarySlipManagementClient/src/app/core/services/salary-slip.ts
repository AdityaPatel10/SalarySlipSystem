import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SalarySlip {
  private apiUrl = `${environment.apiUrl}/MonthlySalarySlips`;

  constructor(private http: HttpClient) {}

  generateSalarySlip(data: {
    employeeGlobalId: string;
    month: number;
    year: number;
  }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/GenerateSalarySlip`, data);
  }

  getEmployeeHistory(employeeGlobalId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetEmployeeHistory/${employeeGlobalId}`);
  }
}
