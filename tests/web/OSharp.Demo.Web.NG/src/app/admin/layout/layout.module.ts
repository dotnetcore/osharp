import { NgModule, } from '@angular/core';
import { LayoutComponent } from './layout.component';
import { LayoutRoutingModule, } from './layout.routing';
import { TranslateModule, } from '@ngx-translate/core';


@NgModule({
    declarations: [
        LayoutComponent,
        // TODO: add components
        // DemoComponent
    ],
    imports: [
        TranslateModule,
        LayoutRoutingModule,
    ],
    providers: [
        // TODO: and services
        // DemoService
    ]
})
export class LayoutModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'layout',
    loadChildren: './layout/layout.module#LayoutModule',
    canActivate: [AuthGuard]
},

 */
