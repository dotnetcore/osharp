import { NgModule, } from '@angular/core';
import { SecurityComponent } from './security.component';
import { SecurityRoutingModule, } from './security.routing';
import { TranslateModule, } from '@ngx-translate/core';

// TODO: import components and services
// import { DemoComponent } from './demo/demo.component';
// import { DemoService } from './demo/demo.service';

@NgModule({
    declarations: [
        SecurityComponent,
        // TODO: add components
        // DemoComponent
    ],
    imports: [
        TranslateModule,
        SecurityRoutingModule,
    ],
    providers: [
        // TODO: and services
        // DemoService
    ]
})
export class SecurityModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'security',
    loadChildren: './security/security.module#SecurityModule',
    canActivate: [AuthGuard]
},

 */
