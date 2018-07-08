import { NgModule, } from '@angular/core';
import { SecurityRoutingModule, } from './security.routing';
import { ModuleComponent } from './module/module.component';
import { FunctionComponent } from './function/function.component';
import { EntityinfoComponent } from './entityinfo/entityinfo.component';
import { UserFunctionComponent } from './user-function/user-function.component';
import { RoleFunctionComponent } from './role-function/role-function.component';
import { UserEntityinfoComponent } from './user-entityinfo/user-entityinfo.component';
import { SharedModule } from '@shared/shared.module';
import { RoleEntityComponent } from './role-entity/role-entity.component';
import { FilterGroupComponent } from './shared/filter-group/filter-group.component';
import { FilterRuleComponent } from './shared/filter-group/filter-rule.component';

const COMPONENTS = [
  ModuleComponent,
  FunctionComponent,
  EntityinfoComponent,
  RoleFunctionComponent,
  UserFunctionComponent,
  RoleEntityComponent,
  UserEntityinfoComponent,
  FilterGroupComponent,
  FilterRuleComponent
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
export class SecurityModule { }
