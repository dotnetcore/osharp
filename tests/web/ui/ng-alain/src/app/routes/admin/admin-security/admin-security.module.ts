import { NgModule, } from '@angular/core';
import { SecurityRoutingModule, } from './admin-security.routing';
import { ModuleComponent } from './module/module.component';
import { FunctionComponent } from './function/function.component';
import { EntityinfoComponent } from './entityinfo/entityinfo.component';
import { UserFunctionComponent } from './user-function/user-function.component';
import { RoleFunctionComponent } from './role-function/role-function.component';
import { RoleEntityinfoComponent } from './role-entityinfo/role-entityinfo.component';
import { UserEntityinfoComponent } from './user-entityinfo/user-entityinfo.component';
import { SharedModule } from '@shared/shared.module';

const COMPONENTS = [
  ModuleComponent,
  FunctionComponent,
  EntityinfoComponent,
  RoleFunctionComponent,
  UserFunctionComponent,
  RoleEntityinfoComponent,
  UserEntityinfoComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    SecurityRoutingModule,
    SharedModule
  ],
  providers: [
  ]
})
export class AdminSecurityModule { }
