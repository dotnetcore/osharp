import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LayoutComponent } from "./layout/layout.component";
import { HomeComponent } from './home/home.component';

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

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminRoutingModule { }
