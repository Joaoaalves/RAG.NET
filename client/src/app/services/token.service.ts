import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private accessTokenKey = 'accessToken';
  private refreshTokenKey = 'refreshToken';
  private expiresInKey = 'expiresIn';

  setAccessToken(token: string): void {
    localStorage.setItem(this.accessTokenKey, token);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  removeAccessToken(): void {
    localStorage.removeItem(this.accessTokenKey);
  }

  setRefreshToken(token: string): void {
    document.cookie = `${this.refreshTokenKey}=${token}; path=/;`;
  }

  getRefreshToken(): string | null {
    const name = this.refreshTokenKey + '=';
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(';');
    for (let c of ca) {
      c = c.trim();
      if (c.indexOf(name) === 0) {
        return c.substring(name.length, c.length);
      }
    }
    return null;
  }

  removeRefreshToken(): void {
    this.deleteCookie(this.refreshTokenKey);
  }

  setExpiresIn(date: Date): void {
    document.cookie = `${this.expiresInKey}=${date.toISOString()}; path=/;`;
  }

  getExpiresIn(): Date | null {
    const name = this.expiresInKey + '=';
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(';');
    for (let c of ca) {
      c = c.trim();
      if (c.indexOf(name) === 0) {
        const dateStr = c.substring(name.length, c.length);
        return new Date(dateStr);
      }
    }
    return null;
  }

  removeExpiresIn(): void {
    this.deleteCookie(this.expiresInKey);
  }

  removeAllTokens(): void {
    this.removeAccessToken();
    this.removeRefreshToken();
    this.removeExpiresIn();
  }

  private deleteCookie(name: string): void {
    document.cookie =
      name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT; path=/;';
  }

  isAccessTokenExpiring(thresholdMinutes: number = 30): boolean {
    const expiresIn = this.getExpiresIn();
    if (!expiresIn) return true;
    const now = new Date();
    const threshold = now.getTime() + thresholdMinutes * 60 * 1000;
    return expiresIn.getTime() <= threshold;
  }
}
