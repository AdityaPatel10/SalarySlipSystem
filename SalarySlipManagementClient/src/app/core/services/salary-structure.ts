import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SalaryStructure {
  private apiUrl = `${environment.apiUrl}/SalaryStructures`;

  constructor(private http: HttpClient) {}

  getAllSalaryStructures(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetAllSalaryStructures`);
  }

  getSalaryStructureById(globalId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/GetSalaryStructureById/${globalId}`);
  }

  createSalaryStructure(rawData: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/CreateNewSalaryStructure`, rawData);
  }

  updateSalaryStructure(globalId: string, updatedData: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/UpdateSalaryStructure/${globalId}`, updatedData, {
      responseType: 'text' as 'json',
    });
  }
}
