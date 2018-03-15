import { NgModule, } from '@angular/core';

import { IdentityRoutingModule, } from './identity.routing';
import { TranslateModule, } from '@ngx-translate/core';
import { RoleComponent } from './role/role.component';
import { UserComponent } from './user/user.component';
import { UserRoleComponent } from './user-role/user-role.component';

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
    providers: []
})
export class IdentityModule { }
