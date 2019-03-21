import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserLoginComponent } from './login/login.component';
import { UserRegisterComponent } from './register/register.component';
import { UserRegisterResultComponent } from './register-result/register-result.component';
import { UserLockComponent } from './lock/lock.component';
import { SharedModule } from '@shared';
import { ForgotPasswordComponent } from './password/forgot-password.component';
import { ResetPasswordComponent } from './password/reset-password.component';
import { PassportRoutingModule } from './passport.routing';
import { SendMailComponent } from './shared/send-mail/send-mail.component';
import { SendConfirmMailComponent } from './email/send-confirm-mail.component';
import { ConfirmEmailComponent } from './email/confirm-email.component';
import { OauthCallbackComponent } from './oauth-callback/oauth-callback.component';

@NgModule({
  declarations: [
    UserLoginComponent,
    UserRegisterComponent,
    UserRegisterResultComponent,
    UserLockComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    SendMailComponent,
    SendConfirmMailComponent,
    ConfirmEmailComponent,
    OauthCallbackComponent
  ],
  imports: [CommonModule, SharedModule, PassportRoutingModule],
  exports: [],
  providers: [],
})
export class PassportModule { }
