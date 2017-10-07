import { NgModule, } from '@angular/core';
import { AdminComponent } from './admin.component';
import { AdminRoutingModule, } from './admin.routing';
import { TranslateModule, } from '@ngx-translate/core';

// TODO: import components and services
// import { DemoComponent } from './demo/demo.component';
// import { DemoService } from './demo/demo.service';

@NgModule({
    declarations: [
        AdminComponent,
        // TODO: add components
        // DemoComponent
    ],
    imports: [
        TranslateModule,
        AdminRoutingModule,
    ],
    providers: [
        // TODO: and services
        // DemoService
    ]
})
export class AdminModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'admin',
    loadChildren: './admin/admin.module#AdminModule',
    canActivate: [AuthGuard]
},

 */
