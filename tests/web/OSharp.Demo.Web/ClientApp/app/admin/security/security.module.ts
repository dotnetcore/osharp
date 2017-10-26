import { NgModule, } from '@angular/core';
import { CommonModule } from "@angular/common";
import { SecurityRoutingModule, } from './security.routing';
import { TranslateModule, } from '@ngx-translate/core';

import { ModuleComponent } from './module/module.component';
import { ModuleService } from './module/module.service';
import { FunctionComponent } from "./function/function.component";
import { FunctionService } from "./function/function.service";
import { RoleFunctionComponent } from './role-function/role-function.component';
import { RoleFunctionService } from './role-function/role-function.service';
import { UserFunctionComponent } from './user-function/user-function.component';
import { UserFunctionService } from "./user-function/user-function.service";
import { EntityinfoComponent } from "./entityinfo/entityinfo.component";
import { EntityinfoService } from "./entityinfo/entityinfo.service";
import { RoleEntityinfoComponent } from "./role-entityinfo/role-entityinfo.component";
import { RoleEntityinfoService } from "./role-entityinfo/role-entityinfo.service";
import { UserEntityinfoComponent } from "./user-entityinfo/user-entityinfo.component";
import { UserEntityinfoService } from "./user-entityinfo/user-entityinfo.service";


@NgModule({
    declarations: [
        ModuleComponent,
        FunctionComponent,
        RoleFunctionComponent,
        UserFunctionComponent,
        EntityinfoComponent,
        RoleEntityinfoComponent,
        UserEntityinfoComponent
    ],
    imports: [
        TranslateModule,
        SecurityRoutingModule,
        CommonModule
    ],
    providers: [
        ModuleService,
        FunctionService,
        RoleFunctionService,
        UserFunctionService,
        EntityinfoService,
        RoleEntityinfoService,
        UserEntityinfoService
    ]
})
export class SecurityModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'security',
    loadChildren: './security/security.module#SecurityModule',
    canActivate: [AuthGuard]
},

 */
