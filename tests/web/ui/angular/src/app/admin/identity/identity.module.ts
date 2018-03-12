import { NgModule, } from '@angular/core';
import { IdentityRoutingModule, } from './identity.routing';
import { TranslateModule, } from '@ngx-translate/core';
import { RoleComponent } from './role/role.component';
import { UserComponent } from './user/user.component';
import { UserRoleComponent } from './user-role/user-role.component';
import { UserService } from './user/user.service';
import { RoleService } from './role/role.service';
import { UserRoleService } from './user-role/user-role.service';

@NgModule({
    declarations: [
        RoleComponent,
        UserComponent,
        UserRoleComponent
    ],
    imports: [
        TranslateModule,
        IdentityRoutingModule,
    ],
    providers: [
        RoleService,
        UserService,
        UserRoleService
    ]
})
export class IdentityModule { }
