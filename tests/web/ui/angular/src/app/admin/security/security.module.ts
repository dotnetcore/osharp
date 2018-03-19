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
import { ModuleService } from './module/module.service';
import { RoleEntityinfoService } from './role-entityinfo/role-entityinfo.service';
import { UserEntityinfoService } from './user-entityinfo/user-entityinfo.service';
import { RoleFunctionService } from './role-function/role-function.service';
import { UserFunctionService } from './user-function/user-function.service';
import { ModuleFunctionComponent } from './module/module-function.component';

@NgModule({
    declarations: [
        ModuleComponent,
        ModuleFunctionComponent,
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
        ModuleService,
        RoleEntityinfoService,
        UserEntityinfoService,
        RoleFunctionService,
        UserFunctionService
    ]
})
export class SecurityModule { }
