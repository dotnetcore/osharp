import { Injectable, Injector } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse } from "@angular/common/http";
import { LoginDto, AjaxResult, OnlineUser, AjaxResultType } from '../../shared/osharp/osharp.model';
import { SettingsService } from '../../shared/angle/core/settings/settings.service';

import { Observable } from 'rxjs/Observable';
import { _throw } from "rxjs/observable/throw";
import { catchError, retry } from "rxjs/operators";
import { AuthTokenService } from './auth-token.service';

@Injectable()
export class AuthService {

  private onlineUser: OnlineUser;

  constructor(private http: HttpClient, public authTokenService: AuthTokenService, public settings: SettingsService) { }

  get GetUser() {
    return this.onlineUser;
  }

  login(dto: LoginDto) {
    let url = "/api/identity/login";
    return this.http.post<AjaxResult>(url, dto).map(result => {
      if (result.Type == AjaxResultType.Success) {
        var user = result.Data;
        this.onlineUser = user;
        sessionStorage.setItem("onlineUser", JSON.stringify(user));
        this.settings.setUserSetting("name", user.UserName);
      }
      return result;
    }).toPromise();
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error("发生错误", error.error.message);
    } else {
      console.error(`Http错误码：${error.status}，详细：${error.error}`);
    }
    return _throw("发生错误，请稍后重试。")
  }
}



