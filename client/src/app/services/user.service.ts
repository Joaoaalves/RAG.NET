import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import { map, Observable } from 'rxjs';
import { UpdateUserRequest } from '../models/update-user-request';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  getInfo(): Observable<User> {
    return this.httpClient
      .get<User>(`${this.apiUrl}/api/info`)
      .pipe(map((user) => user));
  }

  updateInfo(info: UpdateUserRequest): Observable<User> {
    return this.httpClient
      .post<User>(`${this.apiUrl}/api/info`, info)
      .pipe(map((user) => user));
  }
}
