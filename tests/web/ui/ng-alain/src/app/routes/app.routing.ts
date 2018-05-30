import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { environment } from '@env/environment';
// layout
import { LayoutFullScreenComponent } from '../layout/fullscreen/fullscreen.component';
import { LayoutPassportComponent } from '../layout/passport/passport.component';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// passport pages
import { UserLoginComponent } from './passport/login/login.component';
import { UserRegisterComponent } from './passport/register/register.component';
import { UserRegisterResultComponent } from './passport/register-result/register-result.component';
// single pages
import { CallbackComponent } from './callback/callback.component';
import { UserLockComponent } from './passport/lock/lock.component';
import { Exception403Component } from './exception/403.component';
import { Exception404Component } from './exception/404.component';
import { Exception500Component } from './exception/500.component';
import { HomeComponent } from './home/home.component';
import { LayoutDefaultComponent } from '../layout/default/default.component';

const routes: Routes = [
  {
    path: '', component: LayoutDefaultComponent, children: [
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent, data: { title: '首页' } },
    ]
  },
  { path: 'identity', loadChildren: './identity/identity.module#IdentityModule' },
  { path: 'admin', loadChildren: './admin/admin.module#AdminModule' },
  { path: '**', redirectTo: 'home' }
  // { path: '', redirectTo: 'home', pathMatch: 'full' },
  // { path: 'home', component: HomeComponent, data: { title: '首页' } },
  // { path: 'identity', loadChildren: './identity/identity.module#IdentityModule' },
  // { path: 'admin', loadChildren: './admin/admin.module#AdminModule' }
  // {
  //   path: '',
  //   component: LayoutDefaultComponent,
  //   children: [
  //     { path: '', redirectTo: 'home', pathMatch: 'full' },
  //     { path: 'home', component: HomeComponent, data: { title: '网站首页' } },
  //     { path: 'identity', loadChildren: './identity/identity.module#IdentityModule' },
  //   ]
  // },
  // { path: 'admin', loadChildren: './admin/admin.module#AdminModule' },
  // {
  //   path: 'passport',
  //   component: LayoutPassportComponent,
  //   children: [
  //     { path: 'login', component: UserLoginComponent, data: { title: '登录', titleI18n: 'pro-login' } },
  //     { path: 'register', component: UserRegisterComponent, data: { title: '注册', titleI18n: 'pro-register' } },
  //     { path: 'register-result', component: UserRegisterResultComponent, data: { title: '注册结果', titleI18n: 'pro-register-result' } }
  //   ]
  // },
  // // 单页不包裹Layout
  // { path: 'callback/:type', component: CallbackComponent },
  // { path: 'lock', component: UserLockComponent, data: { title: '锁屏', titleI18n: 'lock' } },
  // { path: '403', component: Exception403Component },
  // { path: '404', component: Exception404Component },
  // { path: '500', component: Exception500Component },
  // { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: environment.useHash })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
