import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { AuthHttpInterceptor } from "./auth-http.interceptor";

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }
];
