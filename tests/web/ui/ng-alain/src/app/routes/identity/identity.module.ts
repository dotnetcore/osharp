import { NgModule, } from '@angular/core';
import { CustomFormsModule } from "ng2-validation";
import { IdentityComponent } from './identity.component';
import { IdentityRoutingModule, } from './identity.routing';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ConfirmEmailComponent } from './email/confirm-email.component';
import { SendConfirmMailComponent } from './email/send-confirm-mail.component';
import { ForgotPasswordComponent } from './password/forgot-password.component';
import { ResetPasswordComponent } from './password/reset-password.component';
import { SharedModule } from '@shared/shared.module';
import { IdentityService } from './shared/identity.service';

const COMPONENTS = [
  IdentityComponent,
  LoginComponent,
  RegisterComponent,
  ConfirmEmailComponent,
  SendConfirmMailComponent,
  ForgotPasswordComponent,
  ResetPasswordComponent,
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    CustomFormsModule,
    IdentityRoutingModule,
    SharedModule
  ],
  providers: [
    IdentityService
  ]
})
export class IdentityModule { }
