import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';

import { SecurityRoutingModule, } from './security.routing';
import { KendouiModule } from '../../shared/kendoui/kendoui.module';
import { EntityinfoComponent } from './entityinfo/entityinfo.component';
import { FunctionComponent } from './function/function.component';
import { ModuleComponent } from './module/module.component';
import { RoleEntityinfoComponent } from './role-entityinfo/role-entityinfo.component';
import { UserEntityinfoComponent } from './user-entityinfo/user-entityinfo.component';
import { RoleFunctionComponent } from './role-function/role-function.component';
import { UserFunctionComponent } from './user-function/user-function.component';
import { RoleEntityinfoService } from './role-entityinfo/role-entityinfo.service';
import { UserEntityinfoService } from './user-entityinfo/user-entityinfo.service';

@NgModule({
  declarations: [
    ModuleComponent,
    EntityinfoComponent,
    FunctionComponent,
    RoleEntityinfoComponent,
    UserEntityinfoComponent,
    RoleFunctionComponent,
    UserFunctionComponent
  ],
  imports: [
    TranslateModule,
    SecurityRoutingModule,
    KendouiModule
  ],
  providers: [
    RoleEntityinfoService,
    UserEntityinfoService,
  ]
})
export class SecurityModule { }
