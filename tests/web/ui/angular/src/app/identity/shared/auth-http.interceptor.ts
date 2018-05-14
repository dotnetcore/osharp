import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from "rxjs/operators";
import { AjaxResult, AjaxResultType } from '../../shared/osharp/osharp.model';
import { AuthTokenService } from './auth-token.service';

@Injectable()
export class AuthHttpInterceptor implements HttpInterceptor {

  constructor(public authToken: AuthTokenService, public router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const authToken = this.authToken.getToken();
    let authReq = req;
    if (authToken) {
      authReq = req.clone({
        setHeaders: { "Authorization": authToken }
      });
    }
    return next.handle(authReq).pipe(tap(
      event => {
        console.info("event", event);
        if (event instanceof HttpResponse) {
          if (event.ok && event.body instanceof AjaxResult) {
            let result: AjaxResult = <AjaxResult>event.body;
            switch (result.Type) {
              case AjaxResultType.UnAuth:
                //未登录，跳转到登录页
                this.router.navigate(['/identity/login']);
                break;
              case AjaxResultType.Forbidden:
                //权限不足，跳转到首页
                this.router.navigate(['/']);
                break;
              default:
                break;
            }
            return;
          }
          switch (event.status) {
            case 401:
              //未登录，跳转到登录页
              this.router.navigate(['/identity/login']);
              break;
            case 403:
              //权限不足，跳转到首页
              this.router.navigate(['/']);
              break;
            case 404:
              //不存在，跳转到404页
              break;
            case 500:
              //出错，跳转到错误页
              break;
            default:
              break;
          }
        }
      }
    ));
  }
}
