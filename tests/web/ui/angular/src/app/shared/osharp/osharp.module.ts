import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';
import { OsharpService } from './services/osharp.service';
import { OsharpSettingsService } from './services/osharp.settings.service';
import { AuthGuard } from './services/auth-guard.service';
import { AuthService } from './services/auth.service';
import { AuthTokenService } from './services/auth-token.service';

@NgModule({
  declarations: [
  ],
  imports: [
    TranslateModule,
  ],
  exports: [
  ],
  providers: [
    OsharpService,
    OsharpSettingsService,
    AuthGuard,
    AuthService,
    AuthTokenService
  ]
})
export class OsharpModule { }
