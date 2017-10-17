import { NgModule, } from '@angular/core';
import { CommonModule } from '@angular/common';

import { KendouiSharedModule } from "../shared/kendoui.shared.module";

import { HomeComponent } from './home.component';
import { HomeRoutingModule, } from './home.routing';
import { TranslateModule, } from '@ngx-translate/core';

@NgModule({
    declarations: [
        HomeComponent,
    ],
    imports: [
        TranslateModule,
        CommonModule,
        KendouiSharedModule,
        HomeRoutingModule,
    ],
    providers: [
        // TODO: and services
        // DemoService
    ]
})
export class HomeModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'home',
    loadChildren: './home/home.module#HomeModule',
    canActivate: [AuthGuard]
},

 */
