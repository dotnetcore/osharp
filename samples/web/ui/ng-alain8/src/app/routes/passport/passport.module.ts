import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { RegisterResultComponent } from './register-result/register-result.component';
import { LockComponent } from './lock/lock.component';
import { SendConfirmMailComponent } from './email/send-confirm-mail.component';
import { ConfirmEmailComponent } from './email/confirm-email.component';
import { SendMailComponent } from './shared/send-mail/send-mail.component';
import { OauthCallbackComponent } from './oauth-callback/oauth-callback.component';
import { SharedModule } from '@shared';
import { PassportRoutingModule } from './passport.routing';
import { ForgotPasswordComponent } from './password/forgot-password.component';
import { ResetPasswordComponent } from './password/reset-password.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    PassportRoutingModule,
  ],
  declarations: [
    LoginComponent,
    RegisterComponent,
    RegisterResultComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    LockComponent,
    OauthCallbackComponent,
    SendConfirmMailComponent,
    ConfirmEmailComponent,
    SendMailComponent,
  ],
})
export class PassportModule { }
