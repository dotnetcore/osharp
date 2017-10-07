import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SiteComponent, } from './site.component';
import { HomeComponent } from './home/home.component';
import { DemoComponent } from './demo/demo.component';

// TODO: import components
// import { DemoComponent } from './demo/demo.component';

const routes: Routes = [
    {
        path: '', component: SiteComponent,
        children: [
            // TODO: add route
            { path: 'demo', component: DemoComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SiteRoutingModule { }
