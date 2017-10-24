import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ModuleComponent } from './module/module.component';
import { FunctionComponent } from './function/function.component';
import { RoleFunctionComponent } from './role-function/role-function.component';
import { UserFunctionComponent } from './user-function/user-function.component';
import { EntityinfoComponent } from "./entityinfo/entityinfo.component";
import { RoleEntityinfoComponent } from "./role-entityinfo/role-entityinfo.component";
import { UserEntityinfoComponent } from "./user-entityinfo/user-entityinfo.component";

// TODO: import components
// import { DemoComponent } from './demo/demo.component';

const routes: Routes = [
    { path: 'module', component: ModuleComponent },
    { path: 'function', component: FunctionComponent },
    { path: 'role-function', component: RoleFunctionComponent },
    { path: 'user-function', component: UserFunctionComponent },
    { path: 'entityinfo', component: EntityinfoComponent },
    { path: 'role-entityinfo', component: RoleEntityinfoComponent },
    { path: 'user-entityinfo', component: UserEntityinfoComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SecurityRoutingModule { }
