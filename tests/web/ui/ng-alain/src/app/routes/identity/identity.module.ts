import { NgModule, } from '@angular/core';
import { IdentityComponent } from './identity.component';
import { IdentityRoutingModule, } from './identity.routing';
import { LoginService } from './login/login.service';
import { LoginComponent } from './login/login.component';
import { RegisterService } from './register/register.service';
import { RegisterComponent } from './register/register.component';
import { ConfirmEmailComponent } from './email/confirm-email.component';
import { SendConfirmMailComponent } from './email/send-confirm-mail.component';
import { ForgotPasswordComponent } from './password/forgot-password.component';
import { ResetPasswordComponent } from './password/reset-password.component';

const COMPONENTS = [
  IdentityComponent,
  LoginComponent,
  RegisterComponent,
  ConfirmEmailComponent,
  SendConfirmMailComponent,
  ForgotPasswordComponent,
  ResetPasswordComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    IdentityRoutingModule,
  ],
  providers: [
    LoginService,
    RegisterService
  ]
})
export class IdentityModule { }
