import { Injectable } from '@angular/core';
import { LoginDto, AjaxResult, AjaxResultType } from '../osharp.model';
import { HttpClient } from '@angular/common/http';
import { tokenNotExpired } from "angular2-jwt";
import { AuthTokenService } from './auth-token.service';

@Injectable()
export class AuthService {

  constructor(private http: HttpClient, private authToken: AuthTokenService) { }

  login(dto: LoginDto) {
    let url = "/api/identity/jwtoken";
    return this.http.post<AjaxResult>(url, dto).map(result => {
      if (result.Type == AjaxResultType.Success) {
        this.authToken.set(result.Data);
      }
      return result;
    }).toPromise();
  }

  loggedIn() {
    return tokenNotExpired('id_token');
  }

  logout() {
    let url = '/api/identity/logout';
    return this.http.post<AjaxResult>(url, {}).map(result => {
      this.authToken.remove();
      return result;
    }).toPromise();
  }
}
