import { DOCUMENT } from '@angular/common';
import { Injector } from '@angular/core';
import { Router } from '@angular/router';
import { SimpleTokenModel, JWTTokenModel, DelonAuthConfig, DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';

export function CheckSimple(model: SimpleTokenModel | null): boolean {
  return model != null && typeof model.token === 'string' && model.token.length > 0;
}

export function CheckJwt(model: JWTTokenModel, offset: number): boolean {
  return model != null && !!model.token && !model.isExpired(offset);
}

export function ToLogin(options: DelonAuthConfig, injector: Injector, url?: string) {
  const router = injector.get<Router>(Router);
  (injector.get(DA_SERVICE_TOKEN) as ITokenService).referrer!.url = url || router.url;
  if (options.token_invalid_redirect === true) {
    setTimeout(() => {
      if (/^https?:\/\//g.test(options.login_url!)) {
        injector.get(DOCUMENT).location.href = options.login_url as string;
      } else {
        router.navigate([options.login_url]);
      }
    });
  }
}
