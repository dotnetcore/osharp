import { NgModule, } from '@angular/core';

import { IdentityRoutingModule, } from './identity.routing';
import { TranslateModule, } from '@ngx-translate/core';

import { KendouiModule } from "../../shared/kendoui/kendoui.module";
import { RoleComponent } from './role/role.component';
import { UserComponent } from './user/user.component';
import { UserRoleComponent } from './user-role/user-role.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
    declarations: [
        RoleComponent,
        UserComponent,
        UserRoleComponent
    ],
    imports: [
        TranslateModule,
        IdentityRoutingModule,
        KendouiModule,
        SharedModule
    ],
    providers: []
})
export class IdentityModule { }
