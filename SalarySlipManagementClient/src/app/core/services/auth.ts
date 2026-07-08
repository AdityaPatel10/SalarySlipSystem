import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private apiUrl = 'http://localhost:5045/api/auth';

  constructor(private http: HttpClient) {}

  login(credentials: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials);
  }

  requestOtp(data: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/request-otp`, data);
  }

  verifyOtp(data: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/verify-otp`, data);
  }

  resetPassword(data: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/reset-password`, data);
  }
}
