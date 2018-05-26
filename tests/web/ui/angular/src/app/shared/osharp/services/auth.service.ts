import { Injectable } from '@angular/core';
import { LoginDto, AjaxResult, AjaxResultType } from '../osharp.model';
import { HttpClient } from '@angular/common/http';
import { AuthTokenService } from './auth-token.service';

@Injectable()
export class AuthService {

  constructor(private http: HttpClient, private authToken: AuthTokenService) { }

  login(dto: LoginDto) {
    let url = "/api/identity/token";
    return this.http.post<AjaxResult>(url, dto).map(result => {
      if (result.Type == AjaxResultType.Success) {
        this.authToken.set(result.Data);
      }
      return result;
    }).toPromise();
  }

  loggedIn() {
    return this.authToken.get() != null;
  }

  logout() {
    let url = '/api/identity/logout';
    return this.http.post<AjaxResult>(url, {}).map(result => {
      this.authToken.remove();
      return result;
    }).toPromise();
  }
}
