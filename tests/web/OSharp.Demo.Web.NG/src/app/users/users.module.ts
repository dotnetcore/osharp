import { NgModule, } from '@angular/core';
import { UsersComponent } from './users.component';
import { UsersRoutingModule, } from './users.routing';
import { TranslateModule, } from '@ngx-translate/core';

// TODO: import components and services
// import { DemoComponent } from './demo/demo.component';
// import { DemoService } from './demo/demo.service';

@NgModule({
    declarations: [
        UsersComponent,
        // TODO: add components
        // DemoComponent
    ],
    imports: [
        TranslateModule,
        UsersRoutingModule,
    ],
    providers: [
        // TODO: and services
        // DemoService
    ]
})
export class UsersModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'users',
    loadChildren: './users/users.module#UsersModule',
    canActivate: [AuthGuard]
},

 */
