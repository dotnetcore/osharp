import { NgModule, } from '@angular/core';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule, } from './dashboard.routing';
import { TranslateModule, } from '@ngx-translate/core';

// TODO: import components and services
// import { DemoComponent } from './demo/demo.component';
// import { DemoService } from './demo/demo.service';

@NgModule({
    declarations: [
        DashboardComponent,
        // TODO: add components
        // DemoComponent
    ],
    imports: [
        TranslateModule,
        DashboardRoutingModule,
    ],
    providers: [
        // TODO: and services
        // DemoService
    ]
})
export class DashboardModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'dashboard',
    loadChildren: './dashboard/dashboard.module#DashboardModule',
    canActivate: [AuthGuard]
},

 */
