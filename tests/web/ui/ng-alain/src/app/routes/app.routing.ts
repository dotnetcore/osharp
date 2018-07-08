import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { environment } from '@env/environment';
// single pages
import { CallbackComponent } from './callback/callback.component';
import { Exception403Component } from './exception/403.component';
import { Exception404Component } from './exception/404.component';
import { Exception500Component } from './exception/500.component';
import { HomeComponent } from './home/home.component';
import { LayoutDefaultComponent } from '../layout/default/default.component';
import { TestComponent } from './home/test.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutDefaultComponent,
    children: [
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent, data: { title: '网站首页' } },
      { path: 'test', component: TestComponent, data: { title: '测试页' } },
    ]
  },
  { path: 'identity', loadChildren: './identity/identity.module#IdentityModule' },
  { path: 'admin', loadChildren: './admin/admin.module#AdminModule' },

  // 单页不包裹Layout
  { path: 'callback/:type', component: CallbackComponent },
  { path: '403', component: Exception403Component, data: { title: '无权访问该页面' } },
  { path: '404', component: Exception404Component, data: { title: '页面不存在' } },
  { path: '500', component: Exception500Component, data: { title: '服务器内部错误' } },
  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: environment.useHash })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
