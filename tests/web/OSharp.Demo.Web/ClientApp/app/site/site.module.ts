import { NgModule, } from '@angular/core';
import { SiteComponent } from './site.component';
import { SiteRoutingModule, } from './site.routing';
import { TranslateModule, } from '@ngx-translate/core';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './navmenu/navmenu.component';
import { DemoComponent } from './demo/demo.component';

// TODO: import components and services
// import { DemoComponent } from './demo/demo.component';
// import { DemoService } from './demo/demo.service';

@NgModule({
    declarations: [
        SiteComponent,
        HomeComponent,
        DemoComponent,
        NavMenuComponent
        // TODO: add components
        // DemoComponent
    ],
    imports: [
        TranslateModule,
        SiteRoutingModule,
    ],
    providers: [
        // TODO: and services
        // DemoService
    ]
})
export class SiteModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'site',
    loadChildren: './site/site.module#SiteModule',
    canActivate: [AuthGuard]
},

 */
