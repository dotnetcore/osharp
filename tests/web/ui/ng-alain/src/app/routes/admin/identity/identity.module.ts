import { NgModule, } from '@angular/core';
import { IdentityRoutingModule, } from './identity.routing';
import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';
import { UserRoleComponent } from './user-role/user-role.component';
import { SharedModule } from '@shared/shared.module';

const COMPONENTS = [
  UserComponent,
  RoleComponent,
  UserRoleComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    SharedModule,
    IdentityRoutingModule,
  ],
  providers: [
    // TODO: and services
    // DemoService
  ]
})
export class IdentityModule { }
