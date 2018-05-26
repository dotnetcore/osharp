import { Injectable } from '@angular/core';

@Injectable()
export class AuthTokenService {
  name: string = "id_token";

  get() {
    return localStorage.getItem(this.name);
  }

  set(token: string) {
    if (token) {
      localStorage.setItem(this.name, token);
    }
  }

  remove() {
    localStorage.removeItem(this.name);
  }
}
