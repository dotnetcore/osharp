import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HttpResponseBase, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError, of } from 'rxjs';
import { Optional, Injector, Injectable } from '@angular/core';
import { DelonAuthConfig } from '@delon/auth';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { switchMap, filter, take, mergeMap, catchError } from 'rxjs/operators';
import { AjaxResult, AjaxResultType, JsonWebToken } from '@shared/osharp/osharp.model';


@Injectable()
export class RefreshJWTInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  // Refresh Token Subject tracks the current token, or is null if no token is currently
  // available (e.g. refresh pending).
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(@Optional() protected injector: Injector) { }

  private get identity(): IdentityService {
    return this.injector.get(IdentityService);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const options = { ...new DelonAuthConfig(), ...this.injector.get<DelonAuthConfig>(DelonAuthConfig, undefined) };
    req = this.addToken(req, options, null);

    return next.handle(req).pipe(
      mergeMap((event: any) => {
        if (event instanceof HttpResponseBase) {
          if (event.status === 200 && (event instanceof HttpResponse)) {
            const result: AjaxResult = event.body as AjaxResult;
            if (result && result.Type === AjaxResultType.UnAuth) {
              return this.handle401Error(req, next, options, event);
            }
          }
        }
        return of(event);
      }),
      catchError((error: HttpErrorResponse) => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          return this.handle401Error(req, next, options, error);
        } else {
          return throwError(error);
        }
      })
    );
  }

  /** 给当前Request添加AccessToken */
  private addToken(req: HttpRequest<any>, options: DelonAuthConfig, token: string): HttpRequest<any> {
    if (!token) {
      const model = this.identity.getAccessToken();
      if (model != null && !!model.token && !model.isExpired(options.token_exp_offset)) {
        token = model.token;
      }
    }

    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        }
      });
    }
    return req;
  }

  private handle401Error(req: HttpRequest<any>, next: HttpHandler, options: DelonAuthConfig, event: any) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);
      let refreshToken = this.identity.getRefreshToken();
      if (!refreshToken) {
        this.isRefreshing = false;
        return of(event);
      }
      return this.identity.refreshToken(refreshToken).pipe(
        switchMap((token: any) => {
          this.isRefreshing = false;
          let model = token as AjaxResult;
          if (model && model.Type === AjaxResultType.Success) {
            // 刷新成功
            this.identity.refreshAuth();

            let jwt = model.Data as JsonWebToken;
            this.identity.setToken(jwt);
            this.refreshTokenSubject.next(jwt.AccessToken);
            req = this.addToken(req, options, jwt.AccessToken);
            return next.handle(req);
          }
          return of(event);
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token != null),
        take(1),
        switchMap(jwt => {
          req = this.addToken(req, options, jwt.AccessToken);
          return next.handle(req);
        })
      );
    }
  }
}
