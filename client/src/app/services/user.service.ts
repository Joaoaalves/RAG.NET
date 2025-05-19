import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import { BehaviorSubject, map, Observable, of, tap } from 'rxjs';
import { UpdateUserRequest } from '../models/update-user-request';

const USER_STORAGE_KEY = 'userInfo';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = environment.apiUrl;
  private _user$ = this.createUserSubject();

  readonly user$ = this._user$.asObservable();

  constructor(private httpClient: HttpClient) {}

  private createUserSubject() {
    const stored = localStorage.getItem(USER_STORAGE_KEY);
    const user: User | null = stored ? JSON.parse(stored) : null;
    return new BehaviorSubject<User | null>(user);
  }

  getInfo(forceRefresh = false): Observable<User> {
    const cached = this._user$.getValue();
    if (cached && !forceRefresh) {
      return of(cached);
    }

    return this.httpClient
      .get<User>(`${this.apiUrl}/api/info`)
      .pipe(tap((user) => this.setUserCache(user)));
  }

  updateInfo(info: UpdateUserRequest): Observable<User> {
    return this.httpClient
      .post<User>(`${this.apiUrl}/api/info`, info)
      .pipe(tap((user) => this.setUserCache(user)));
  }

  private setUserCache(user: User) {
    const basic: User = {
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email,
    };
    localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(basic));
    this._user$.next(basic);
  }

  readonly userInitials$: Observable<string> = this.user$.pipe(
    map((user) => {
      if (!user) return '';
      const f = user.firstName?.trim().charAt(0) ?? '';
      const l = user.lastName?.trim().charAt(0) ?? '';
      return (f + l).toUpperCase();
    })
  );

  clearCache() {
    localStorage.removeItem(USER_STORAGE_KEY);
    this._user$.next(null);
  }
}
