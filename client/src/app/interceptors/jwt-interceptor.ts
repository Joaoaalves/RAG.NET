import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './../services/auth.service';
import { TokenService } from './../services/token.service';
import { switchMap, catchError } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private tokenService: TokenService
  ) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (req.url.includes('/refresh')) {
      return next.handle(req);
    }

    if (!this.authService.isLoggedIn()) {
      return next.handle(req);
    }

    if (this.tokenService.isAccessTokenExpiring()) {
      return this.authService.refreshToken().pipe(
        switchMap((response) => {
          const clonedReq = req.clone({
            setHeaders: {
              Authorization: `Bearer ${response.accessToken}`,
            },
          });
          return next.handle(clonedReq);
        }),
        catchError(() => {
          this.authService.logout();
          return next.handle(req);
        })
      );
    } else {
      const accessToken = this.tokenService.getAccessToken();
      const clonedReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`,
        },
      });
      return next.handle(clonedReq);
    }
  }
}
