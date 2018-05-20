import { Injectable } from '@angular/core';
import { tokenNotExpired } from 'angular2-jwt';
import { LoginDto, AjaxResult, AjaxResultType } from '../osharp.model';
import { HttpClient } from '@angular/common/http';


@Injectable()
export class AuthService {
  constructor(private http: HttpClient) { }

  login(dto: LoginDto) {
    let url = "/api/identity/login";
    return this.http.post<AjaxResult>(url, dto).map(result => {
      if (result.Type == AjaxResultType.Success) {
        localStorage.setItem('id_token', result.Data);
      }
      return result;
    }).toPromise();
  }

  loggedIn() {
    return tokenNotExpired();
  }

  logout() {
    let url = '/api/identity/logout';
    return this.http.post<AjaxResult>(url, {}).map(result => {
      if (result.Type == AjaxResultType.Success) {
        localStorage.removeItem('id_token');
      }
      return result;
    }).toPromise();
  }
}
