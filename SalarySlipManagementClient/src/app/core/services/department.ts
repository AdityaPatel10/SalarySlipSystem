import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Department {
  private apiUrl = 'http://localhost:5045/api/Departments';

  constructor(private http: HttpClient) {}

  getAllDepartments(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetAllDepartments`);
  }

  createNewDepartment(name: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/CreateNewDepartment`, { name });
  }
}
