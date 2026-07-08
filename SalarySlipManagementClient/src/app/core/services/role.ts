import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class Role {
  private apiUrl = `${environment.apiUrl}/Roles`;

  constructor(private http: HttpClient) {}

  getAllRoles(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetAllRoles`);
  }

  createNewRole(roleName: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/CreateNewRole`, { roleName });
  }

  updateRole(globalId: string, roleName: string): Observable<any> {
    return this.http.put(
      `${this.apiUrl}/UpdateRole/${globalId}`,
      { roleName },
      { responseType: 'text' },
    );
  }

  deleteRole(globalId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteRole/${globalId}`, { responseType: 'text' });
  }
}
