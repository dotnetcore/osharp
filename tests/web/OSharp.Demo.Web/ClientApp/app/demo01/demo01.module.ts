import { NgModule, } from '@angular/core';
import { Demo01Component } from './demo01.component';
import { Demo01RoutingModule, } from './demo01.routing';
import { TranslateModule, } from '@ngx-translate/core';

// TODO: import components and services
// import { DemoComponent } from './demo/demo.component';
// import { DemoService } from './demo/demo.service';

@NgModule({
    declarations: [
        Demo01Component,
        // TODO: add components
        // DemoComponent
    ],
    imports: [
        TranslateModule,
        Demo01RoutingModule,
    ],
    providers: [
        // TODO: and services
        // DemoService
    ]
})
export class Demo01Module { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'demo01',
    loadChildren: './demo01/demo01.module#Demo01Module',
    canActivate: [AuthGuard]
},

 */
