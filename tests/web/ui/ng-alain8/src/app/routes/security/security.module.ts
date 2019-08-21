import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SecurityRoutingModule } from './security.routing';
import { ModuleComponent } from './module/module.component';
import { FunctionComponent } from './function/function.component';
import { EntityinfoComponent } from './entityinfo/entityinfo.component';
import { RoleFunctionComponent } from './role-function/role-function.component';
import { RoleEntityinfoComponent } from './role-entityinfo/role-entityinfo.component';
import { UserFunctionComponent } from './user-function/user-function.component';
import { SharedModule } from '@shared';


@NgModule({
  declarations: [ModuleComponent, FunctionComponent, EntityinfoComponent, RoleFunctionComponent, RoleEntityinfoComponent, UserFunctionComponent],
  imports: [
    CommonModule,
    SharedModule,
    SecurityRoutingModule
  ],
  entryComponents: [ModuleComponent]
})
export class SecurityModule { }
