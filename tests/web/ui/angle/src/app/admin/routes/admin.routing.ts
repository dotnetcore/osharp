import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LayoutComponent } from '../layout/layout.component';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { MenuService } from '../../shared/angle/core/menu/menu.service';
import { menu } from './menu';


const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardComponent, data: { title: '信息汇总 - 管理' } },
            { path: 'identity', loadChildren: '../identity/identity.module#IdentityModule' },
            { path: 'security', loadChildren: '../security/security.module#SecurityModule' },
            { path: 'system', loadChildren: '../system/system.module#SystemModule' },
            { path: 'material', loadChildren: '../material/material.module#MaterialModule' },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminRoutingModule {
    constructor(public menuService: MenuService) {
        menuService.addMenu(menu);
    }
}
