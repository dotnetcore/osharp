import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';

import { MenuService } from '../angle/menu/menu.service';
import { menu } from "./menu";
import { LayoutComponent } from '../layout/layout.component';
import { HomeComponent } from '../home/home.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: HomeComponent },
            { path: 'identity', loadChildren: '../identity/identity.module#IdentityModule' }
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
