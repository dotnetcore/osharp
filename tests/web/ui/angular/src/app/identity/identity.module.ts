import { NgModule, } from '@angular/core';
import { IdentityComponent } from './identity.component';
import { IdentityRoutingModule, } from './identity.routing';
import { TranslateModule, } from '@ngx-translate/core';
import { CustomFormsModule } from "ng2-validation";
import { SharedModule } from "../shared/shared.module";
import { AngleModule } from "../shared/angle/angle.module";
import { AuthService } from './shared/auth.service';

import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { ResetPasswordComponent } from "./reset-password/reset-password.component";
import { ForgotPasswordComponent } from './reset-password/forgot-password.component';
import { SendConfirmMailComponent } from './confirm-email/send-confirm-mail.component';
import { AuthTokenService } from './shared/auth-token.service';

@NgModule({
  declarations: [
    IdentityComponent,
    LoginComponent,
    RegisterComponent,
    ConfirmEmailComponent,
    ResetPasswordComponent,
    ForgotPasswordComponent,
    SendConfirmMailComponent
  ],
  imports: [
    TranslateModule,
    IdentityRoutingModule,
    SharedModule,
    AngleModule,
    CustomFormsModule
  ],
  providers: [
    AuthService,
    AuthTokenService
  ]
})
export class IdentityModule { }
