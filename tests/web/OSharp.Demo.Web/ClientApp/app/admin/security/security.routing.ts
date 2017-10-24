import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SecurityComponent, } from './security.component';

// TODO: import components
// import { DemoComponent } from './demo/demo.component';

const routes: Routes = [
    {
        path: '', component: SecurityComponent,
        children: [
            // TODO: add route
            // { path: 'demo', component: DemoComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SecurityRoutingModule { }
