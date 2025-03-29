import { LoginResponse } from './../models/login-response';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../models/login-request';
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl;
  constructor(private httpClient: HttpClient) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        map((response) => {
          localStorage.setItem('accessToken', response.accessToken);
          document.cookie = `refreshToken=${response.refreshToken}`;
          return response;
        })
      );
  }

  refreshToken(): Observable<LoginResponse> {
    const refreshToken = this.getRefreshTokenFromCookie();

    return this.httpClient
      .post<LoginResponse>(`${this.apiUrl}/refresh`, { refreshToken })
      .pipe(
        map((response) => {
          localStorage.setItem('accessToken', response.accessToken);
          document.cookie = `refreshToken=${response.refreshToken}`;
          return response;
        })
      );
  }

  logout(): void {
    localStorage.removeItem('accessToken');
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('accessToken') !== null;
  }

  private getRefreshTokenFromCookie(): string | null {
    const cookieString: string = document.cookie;
    const cookieArray = cookieString.split('; ');

    for (const cookie of cookieArray) {
      const [name, value] = cookie.split('=');

      if (name == 'refreshToken') {
        return value;
      }
    }

    return null;
  }
}
