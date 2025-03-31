import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { LoginRequest } from '../models/login-request';
import { RegisterRequest } from '../models/register-request';
import { LoginResponse } from '../models/login-response';
import { Observable, of, throwError } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl;
  constructor(
    private httpClient: HttpClient,
    private tokenService: TokenService
  ) {}

  register(credentials: RegisterRequest): Observable<boolean> {
    return this.httpClient
      .post<void>(`${this.apiUrl}/api/register`, credentials)
      .pipe(
        map(() => {
          this.login(credentials);
          return true;
        }),
        catchError(() => of(false))
      );
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        map((response) => {
          const expiresIn = new Date();
          expiresIn.setSeconds(expiresIn.getSeconds() + response.expiresIn);
          this.tokenService.setAccessToken(response.accessToken);
          this.tokenService.setRefreshToken(response.refreshToken);
          this.tokenService.setExpiresIn(expiresIn);
          return response;
        })
      );
  }

  refreshToken(): Observable<LoginResponse> {
    const refreshToken = this.tokenService.getRefreshToken();

    return this.httpClient
      .post<LoginResponse>(`${this.apiUrl}/refresh`, { refreshToken })
      .pipe(
        map((response) => {
          const expiresIn = new Date();
          expiresIn.setSeconds(expiresIn.getSeconds() + response.expiresIn);

          this.tokenService.setAccessToken(response.accessToken);
          this.tokenService.setRefreshToken(response.refreshToken);
          this.tokenService.setExpiresIn(expiresIn);
          return response;
        }),
        catchError((error) => {
          console.error('Error refreshing token', error);
          return throwError(error);
        })
      );
  }

  logout(): void {
    this.tokenService.removeAllTokens();
  }

  isLoggedIn(): boolean {
    const hasAccessToken = this.tokenService.getAccessToken() !== null;
    const hasRefreshToken = this.tokenService.getRefreshToken() !== null;
    const expiresIn = this.tokenService.getExpiresIn();
    return (
      hasAccessToken &&
      hasRefreshToken &&
      expiresIn !== null &&
      expiresIn > new Date()
    );
  }
}
