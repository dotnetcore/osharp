import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LayoutAdminComponent } from '../../../layout/admin/admin.component';
import { MenuService } from '@delon/theme';
import { adminMenu } from "./admin-menu";

const routes: Routes = [
  {
    path: '', component: LayoutAdminComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, data: { title: '信息汇总', reuse: true } },
      { path: 'identity', loadChildren: './identity/identity.module#IdentityModule' },
      { path: 'security', loadChildren: './security/security.module#SecurityModule' },
      { path: 'system', loadChildren: './system/system.module#SystemModule' },
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
    console.log(menuService.menus);
  }
}
