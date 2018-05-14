import { Injectable } from '@angular/core';

@Injectable()
export class AuthTokenService {
  private token: string;
  getToken() {
    return this.token;
  }
  setToken(token: string) {
    this.token = token;
  }
}
