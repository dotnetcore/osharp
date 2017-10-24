import { NgModule, } from '@angular/core';
import { IdentityRoutingModule, } from './identity.routing';
import { TranslateModule, } from '@ngx-translate/core';

import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';
import { UserService } from './user/user.service';
import { RoleService } from './role/role.service';
import { UserRoleComponent } from './user-role/user-role.component';
import { UserRoleService } from "./user-role/user-role.service";

@NgModule({
    declarations: [
        UserComponent,
        RoleComponent,
        UserRoleComponent
    ],
    imports: [
        TranslateModule,
        IdentityRoutingModule,
    ],
    providers: [
        UserService,
        RoleService,
        UserRoleService
    ]
})
export class IdentityModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'identity',
    loadChildren: './identity/identity.module#IdentityModule',
    canActivate: [AuthGuard]
},

 */
