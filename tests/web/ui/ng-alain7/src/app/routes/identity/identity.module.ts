import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';
import { UserRoleComponent } from './user-role/user-role.component';
import { SharedModule } from '@shared';
import { IdentityRoutingModule } from './identity.routing';

import '@progress/kendo-ui/js/kendo.web.js';
import '@progress/kendo-ui/js/cultures/kendo.culture.zh-CN';
import '@progress/kendo-ui/js/messages/kendo.messages.zh-CN';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    IdentityRoutingModule
  ],
  declarations: [
    UserComponent,
    RoleComponent,
    UserRoleComponent,
  ]
})
export class IdentityModule { }
