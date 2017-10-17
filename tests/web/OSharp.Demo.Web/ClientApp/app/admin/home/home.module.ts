import { NgModule, } from '@angular/core';
import { HomeComponent } from './home.component';
import { HomeRoutingModule, } from './home.routing';
import { TranslateModule, } from '@ngx-translate/core';

// TODO: import components and services
// import { DemoComponent } from './demo/demo.component';
// import { DemoService } from './demo/demo.service';

@NgModule({
    declarations: [
        HomeComponent,
        // TODO: add components
        // DemoComponent
    ],
    imports: [
        TranslateModule,
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
