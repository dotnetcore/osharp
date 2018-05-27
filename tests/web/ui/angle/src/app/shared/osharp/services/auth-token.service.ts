import { Injectable } from '@angular/core';
import { User } from '../osharp.model';

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

  user(): User {
    let token = this.get();
    if (!token) {
      return null;
    }
    return new User(token);
  }
}
