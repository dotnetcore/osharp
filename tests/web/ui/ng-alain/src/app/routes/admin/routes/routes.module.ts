import { NgModule, } from '@angular/core';
import { AdminRoutingModule, } from './admin.routing';
import { DashboardComponent } from './dashboard/dashboard.component';

// TODO: import components and services
// import { DemoComponent } from './demo/demo.component';
// import { DemoService } from './demo/demo.service';

@NgModule({
  declarations: [
    DashboardComponent
  ],
  imports: [
    AdminRoutingModule
  ],
  providers: [
    // TODO: and services
    // DemoService
  ]
})
export class RoutesModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'routes',
    loadChildren: './routes/routes.module#RoutesModule',
    canActivate: [AuthGuard]
},

 */
