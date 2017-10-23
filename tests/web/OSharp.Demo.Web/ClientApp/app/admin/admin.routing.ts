import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LayoutComponent } from "./layout/layout.component";
import { HomeComponent } from './home/home.component';
import { MenuService } from "./angle/menu/menu.service";

// TODO: import components
// import { DemoComponent } from './demo/demo.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            // TODO: add route
            // { path: 'demo', component: DemoComponent },
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
        ]
    }
];

//default menu
const home = { text: '主页', link: './home', icon: 'icon-home' };
const headingMain = { text: '导航菜单', heading: true };
export const mainMenu = [headingMain, home];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminRoutingModule {
    constructor(public menuService: MenuService) {
        menuService.addMenu(mainMenu);
    }
}
