import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Role {
  private apiUrl = 'http://localhost:5045/api/Roles';

  constructor(private http: HttpClient) {}

  getAllRoles(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetAllRoles`);
  }

  createNewRole(name: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/CreateNewRole`, { name });
  }
}
