import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ModuleComponent } from './module/module.component';
import { FunctionComponent } from './function/function.component';
import { RoleFunctionComponent } from './role-function/role-function.component';
import { UserFunctionComponent } from './user-function/user-function.component';
import { EntityinfoComponent } from './entityinfo/entityinfo.component';
import { RoleEntityinfoComponent } from './role-entityinfo/role-entityinfo.component';
import { UserEntityinfoComponent } from './user-entityinfo/user-entityinfo.component';

const routes: Routes = [
    { path: 'module', component: ModuleComponent, data: { title: '模块信息 - 管理' } },
    { path: 'function', component: FunctionComponent, data: { title: '功能信息 - 管理' } },
    { path: 'role-function', component: RoleFunctionComponent, data: { title: '角色功能信息 - 管理' } },
    { path: 'user-function', component: UserFunctionComponent, data: { title: '用户功能信息 - 管理' } },
    { path: 'entityinfo', component: EntityinfoComponent, data: { title: '实体信息 - 管理' } },
    { path: 'role-entityinfo', component: RoleEntityinfoComponent, data: { title: '角色实体信息 - 管理' } },
    { path: 'user-entityinfo', component: UserEntityinfoComponent, data: { title: '用户实体信息 - 管理' } }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SecurityRoutingModule { }
