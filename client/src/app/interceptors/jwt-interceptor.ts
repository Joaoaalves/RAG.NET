import { Observable } from 'rxjs';
import { AuthService } from './../services/auth.service';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const isLoggedIn = this.authService.isLoggedIn();

    if (isLoggedIn) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
        },
      });
    }

    return next.handle(req);
  }
}
