import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { environment } from '@env/environment';
// layout
import { LayoutDefaultComponent } from '../layout/default/default.component';
import { LayoutFullScreenComponent } from '../layout/fullscreen/fullscreen.component';
import { LayoutPassportComponent } from '../layout/passport/passport.component';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// single pages
import { CallbackComponent } from './callback/callback.component';
import { ACLGuard } from '@delon/acl';
import { SimpleGuard } from '@delon/auth';

const routes: Routes = [
  {
    path: '',
    component: LayoutDefaultComponent,
    canActivate: [SimpleGuard],
    data: { title: '主页' },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, canActivate: [ACLGuard], data: { title: '仪表盘', titleI18n: "menu.nav.home", guard: 'Root.Admin.Dashboard' } },
      { path: 'profile', loadChildren: './profile/profile.module#ProfileModule' },
      { path: 'exception', loadChildren: './exception/exception.module#ExceptionModule' },
      // 业务子模块
      { path: 'identity', loadChildren: './identity/identity.module#IdentityModule', canActivateChild: [ACLGuard], data: { guard: 'Root.Admin.Identity' } },
      { path: 'security', loadChildren: './security/security.module#SecurityModule', canActivateChild: [ACLGuard], data: { guard: 'Root.Admin.Security' } },
      { path: 'systems', loadChildren: './systems/systems.module#SystemsModule', canActivateChild: [ACLGuard], data: { guard: 'Root.Admin.Systems' } },
    ]
  },
  // 全屏布局
  {
    path: 'fullscreen',
    component: LayoutFullScreenComponent,
    children: [
    ]
  },
  // passport
  {
    path: 'passport',
    component: LayoutPassportComponent,
    loadChildren: './passport/passport.module#PassportModule'
  },
  // 单页不包裹Layout
  { path: 'callback/:type', component: CallbackComponent },
  { path: '**', redirectTo: 'exception/404' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(
      routes, {
        useHash: environment.useHash,
        // NOTICE: If you use `reuse-tab` component and turn on keepingScroll you can set to `disabled`
        // Pls refer to https://ng-alain.com/components/reuse-tab
        scrollPositionRestoration: 'disabled',
        // scrollPositionRestoration: 'top',
      }
    )],
  exports: [RouterModule],
})
export class RouteRoutingModule { }
