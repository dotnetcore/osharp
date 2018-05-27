import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IdentityComponent, } from './identity.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ForgotPasswordComponent } from './reset-password/forgot-password.component';
import { SendConfirmMailComponent } from './confirm-email/send-confirm-mail.component';

// TODO: import components
// import { DemoComponent } from './demo/demo.component';

const routes: Routes = [
  {
    path: '', component: IdentityComponent,
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: LoginComponent, data: { title: '用户登录' } },
      { path: 'register', component: RegisterComponent, data: { title: '新用户注册' } },
      { path: 'confirm-email', component: ConfirmEmailComponent, data: { title: '注册邮箱激活' } },
      { path: 'send-confirm-mail', component: SendConfirmMailComponent, data: { title: '重发激活邮件' } },
      { path: 'forgot-password', component: ForgotPasswordComponent, data: { title: '找回登录密码' } },
      { path: 'reset-password', component: ResetPasswordComponent, data: { title: '重置登录密码' } },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityRoutingModule { }
