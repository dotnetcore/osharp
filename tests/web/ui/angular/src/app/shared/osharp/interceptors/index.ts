import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { AuthHttpInterceptor } from "../../../identity/shared/auth-http.interceptor";

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }
];
