import { NgModule, } from '@angular/core';
import { IdentityRoutingModule, } from './identity.routing';
import { TranslateModule, } from '@ngx-translate/core';

import { UserComponent } from './user/user.component';
import { UserService } from './user/user.service';
import { RoleComponent } from './role/role.component';
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
