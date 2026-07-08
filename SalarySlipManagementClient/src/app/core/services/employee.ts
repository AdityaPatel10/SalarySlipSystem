import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class Employee {
  private apiUrl = `${environment.apiUrl}/Employees`;

  constructor(private http: HttpClient) {}

  //These are the response request

  createNewEmployee(rawData: any): Observable<any> {
    const formattedPayload = {
      ...rawData,
      departmentGlobalId: rawData.departmentId,
      roleGlobalId: rawData.roleId,
    };

    return this.http.post<any>(`${this.apiUrl}/CreateNewEmployee`, formattedPayload);
  }

  getAllEmployees(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetAllEmployees`);
  }

  getEmployeeById(globalId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/GetEmployeeById/${globalId}`);
  }

  updateEmployee(globalId: string, updatedData: any): Observable<any> {
    const formattedPayload = {
      ...updatedData,
      departmentGlobalId: updatedData.departmentId,
      roleGlobalId: updatedData.roleId,
    };

    return this.http.put(`${this.apiUrl}/UpdateEmployee/${globalId}`, formattedPayload, {
      responseType: 'text',
    });
  }

  deleteEmployee(globalId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteEmployee/${globalId}`, {
      responseType: 'text',
    });
  }

  updateSettings(data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/settings`, data);
  }
}
