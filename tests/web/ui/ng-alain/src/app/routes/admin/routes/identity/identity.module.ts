import { NgModule, } from '@angular/core';
import { IdentityRoutingModule, } from './identity.routing';
import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';
import { UserRoleComponent } from './user-role/user-role.component';

const Components = [
  UserComponent,
  RoleComponent,
  UserRoleComponent
];

@NgModule({
  declarations: [
    ...Components
  ],
  imports: [
    IdentityRoutingModule,
  ],
  providers: [
  ]
})
export class IdentityModule { }
