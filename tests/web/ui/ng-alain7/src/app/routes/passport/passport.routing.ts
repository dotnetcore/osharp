import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { UserLoginComponent } from './login/login.component';
import { UserRegisterComponent } from './register/register.component';
import { UserRegisterResultComponent } from './register-result/register-result.component';
import { UserLockComponent } from './lock/lock.component';
import { ForgotPasswordComponent } from './password/forgot-password.component';

const routes: Routes = [
  { path: 'login', component: UserLoginComponent, data: { title: '用户登录', titleI18n: "app.passport.login" } },
  { path: 'register', component: UserRegisterComponent, data: { title: '新用户注册', titleI18n: "app.passport.register" } },
  { path: 'register-result', component: UserRegisterResultComponent, data: { title: '注册结果', titleI18n: "app.passport.register" } },
  { path: 'forgot-password', component: ForgotPasswordComponent, data: { title: '忘记密码', titleI18n: "app.passport.forgot-password" } },
  { path: 'lock', component: UserLockComponent, data: { title: '锁屏', titleI18n: "app.passport.lock" } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PassportRoutingModule { }
