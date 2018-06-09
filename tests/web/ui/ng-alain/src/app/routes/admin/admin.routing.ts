import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MenuService } from '@delon/theme';
import { adminMenu } from "./admin-menu";
import { AdminLayoutComponent } from './layout/layout.component';

const routes: Routes = [
  {
    path: '', component: AdminLayoutComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, data: { title: '信息汇总', reuse: true } },
      { path: 'identity', loadChildren: '../admin/identity/identity.module#IdentityModule' },
      { path: 'security', loadChildren: '../admin/security/security.module#SecurityModule' },
      { path: 'system', loadChildren: '../admin/system/system.module#SystemModule' },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
  constructor(public menuService: MenuService) {
    menuService.add(adminMenu);
  }
}
