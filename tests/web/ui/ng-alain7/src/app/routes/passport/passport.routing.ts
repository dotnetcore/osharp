import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { UserLoginComponent } from './login/login.component';
import { UserRegisterComponent } from './register/register.component';
import { UserRegisterResultComponent } from './register-result/register-result.component';
import { UserLockComponent } from './lock/lock.component';
import { ForgotPasswordComponent } from './password/forgot-password.component';
import { SendConfirmMailComponent } from './email/send-confirm-mail.component';
import { ConfirmEmailComponent } from './email/confirm-email.component';
import { ResetPasswordComponent } from './password/reset-password.component';
import { OauthCallbackComponent } from './oauth-callback/oauth-callback.component';

const routes: Routes = [
  { path: 'login', component: UserLoginComponent, data: { title: '用户登录', titleI18n: "app.passport.login" } },
  { path: 'register', component: UserRegisterComponent, data: { title: '新用户注册', titleI18n: "app.passport.register" } },
  { path: 'register-result', component: UserRegisterResultComponent, data: { title: '注册结果', titleI18n: "app.passport.register" } },
  { path: 'send-confirm-mail', component: SendConfirmMailComponent, data: { title: '重发激活邮件', titleI18n: "app.passport.send-confirm-mail" } },
  { path: 'confirm-email', component: ConfirmEmailComponent, data: { title: '激活注册邮箱', titleI18n: "app.passport.confirm-email" } },
  { path: 'forgot-password', component: ForgotPasswordComponent, data: { title: '忘记密码', titleI18n: "app.passport.forgot-password" } },
  { path: 'reset-password', component: ResetPasswordComponent, data: { title: '重置密码', titleI18n: "app.passport.reset-password" } },
  { path: 'oauth-callback', component: OauthCallbackComponent, data: { title: '第三方登录', titleI18n: "app.passport.oauth" } },
  { path: 'lock', component: UserLockComponent, data: { title: '锁屏', titleI18n: "app.passport.lock" } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PassportRoutingModule { }
