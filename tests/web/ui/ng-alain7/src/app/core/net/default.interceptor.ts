import { Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse, HttpEvent, HttpResponseBase, HttpResponse, HttpSentEvent, HttpHeaderResponse, HttpProgressEvent, HttpUserEvent } from '@angular/common/http';
import { Observable, of, throwError, BehaviorSubject } from 'rxjs';
import { mergeMap, catchError } from 'rxjs/operators';
import { NzMessageService, NzNotificationService } from 'ng-zorro-antd';
import { _HttpClient } from '@delon/theme';
import { environment } from '@env/environment';
import { DA_SERVICE_TOKEN, ITokenService, JWTTokenModel } from '@delon/auth';
import { AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';

const CODEMESSAGE = {
  200: '服务器成功返回请求的数据。',
  201: '新建或修改数据成功。',
  202: '一个请求已经进入后台排队（异步任务）。',
  204: '删除数据成功。',
  400: '发出的请求有错误，服务器没有进行新建或修改数据的操作。',
  401: '用户没有权限（令牌、用户名、密码错误）。',
  403: '用户得到授权，但是访问是被禁止的。',
  404: '发出的请求针对的是不存在的记录，服务器没有进行操作。',
  406: '请求的格式不可得。',
  410: '请求的资源被永久删除，且不会再得到的。',
  422: '当创建一个对象时，发生一个验证错误。',
  500: '服务器发生错误，请检查服务器。',
  502: '网关错误。',
  503: '服务不可用，服务器暂时过载或维护。',
  504: '网关超时。',
};

/**
 * 默认HTTP拦截器，其注册细节见 `app.module.ts`
 */
@Injectable()
export class DefaultInterceptor implements HttpInterceptor {

  tokenSrv: ITokenService;
  isRefreshingToken = false;
  tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  constructor(private injector: Injector) {
    this.tokenSrv = injector.get(DA_SERVICE_TOKEN) as ITokenService;
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpSentEvent | HttpHeaderResponse | HttpProgressEvent | HttpResponse<any> | HttpUserEvent<any>> {
    // 统一加上服务端前缀
    let url = req.url;
    if (!url.startsWith('https://') && !url.startsWith('http://')) {
      url = environment.SERVER_URL + url;
    }
    req = req.clone({ url });

    // 设置 JWT-Token
    let tokenModel = this.tokenSrv.get<JWTTokenModel>(JWTTokenModel);
    if (tokenModel && tokenModel.token) {
      req = this.addToken(req, tokenModel.token);
    }

    return next.handle(req).pipe(
      mergeMap((event: any) => {
        // 允许统一对请求错误处理
        if (event instanceof HttpResponseBase)
          return this.handleData(event);
        // 若一切都正常，则后续操作
        return of(event);
      }),
      catchError((err: HttpErrorResponse) => this.handleData(err)),
    );
  }

  get msg(): NzMessageService {
    return this.injector.get(NzMessageService);
  }

  private goTo(url: string) {
    setTimeout(() => this.injector.get(Router).navigateByUrl(url));
  }

  private checkStatus(ev: HttpResponseBase) {
    if (ev.status >= 200 && ev.status < 300) return;

    const errortext = CODEMESSAGE[ev.status] || ev.statusText;
    this.injector.get(NzNotificationService).error(
      `请求错误 ${ev.status}: ${ev.url}`,
      errortext
    );
  }

  private addToken(req: HttpRequest<any>, token: string): HttpRequest<any> {
    return req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }

  private handleData(ev: HttpResponseBase): Observable<any> {
    const loginUrl = '/passport/login';
    // 可能会因为 `throw` 导出无法执行 `_HttpClient` 的 `end()` 操作
    if (ev.status > 0) {
      this.injector.get(_HttpClient).end();
    }
    this.checkStatus(ev);
    // 业务处理：一些通用操作
    switch (ev.status) {
      case 200:
        if (ev instanceof HttpResponse) {
          const result = ev.body as AjaxResult;
          if (result && result.Type) {
            switch (result.Type) {
              case AjaxResultType.Success:
              case AjaxResultType.Info:
              case AjaxResultType.Error:
              case AjaxResultType.Locked:
                this.setToken(ev);
                return of(ev);
              case AjaxResultType.UnAuth:
                this.msg.warning('用户未登录或者登录已过期');
                this.goTo(loginUrl);
                break;
              default:
                const type = result.Type as number;
                this.goTo(`/exception/${type}`);
                break;
            }
          } else {
            this.setToken(ev);
          }
        }
        break;
      case 401: // 未登录状态码
        // 请求错误 401: https://preview.pro.ant.design/api/401 用户没有权限（令牌、用户名、密码错误）。
        this.tokenSrv.clear();
        this.goTo('/passport/login');
        break;
      case 403:
        let token = this.tokenSrv.get<JWTTokenModel>();
        if (!token || !token.token || token.isExpired) {
          this.tokenSrv.clear();
          this.goTo('/passport/login');
        } else {
          this.goTo(`/exception/${ev.status}`);
        }
        break;
      case 404:
      case 500:
        this.goTo(`/exception/${ev.status}`);
        break;
      default:
        if (ev instanceof HttpErrorResponse) {
          console.warn('未可知错误，大部分是由于后端不支持CORS或无效配置引起', ev);
          return throwError(ev);
        }
        break;
    }
    return of(ev);
  }

  private setToken(event: HttpResponse<any>) {
    let token = event.headers.get('set-authorization');
    if (token) {
      this.tokenSrv.clear();
      this.tokenSrv.set({ token });
    }
  }

}
