import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ModuleComponent } from './module/module.component';
import { FunctionComponent } from './function/function.component';
import { RoleFunctionComponent } from './role-function/role-function.component';
import { UserFunctionComponent } from './user-function/user-function.component';
import { EntityinfoComponent } from './entityinfo/entityinfo.component';
import { RoleEntityComponent } from './role-entity/role-entity.component';
import { UserEntityinfoComponent } from './user-entityinfo/user-entityinfo.component';

const routes: Routes = [
  { path: 'module', component: ModuleComponent, data: { title: '模块信息管理', reuse: true } },
  { path: 'function', component: FunctionComponent, data: { title: '功能信息管理', reuse: true } },
  { path: 'role-function', component: RoleFunctionComponent, data: { title: '角色功能管理', reuse: true } },
  { path: 'user-function', component: UserFunctionComponent, data: { title: '用户功能管理', reuse: true } },
  { path: 'entityinfo', component: EntityinfoComponent, data: { title: '实体管理', reuse: true } },
  { path: 'role-entity', component: RoleEntityComponent, data: { title: '角色数据管理', reuse: true } },
  { path: 'user-entityinfo', component: UserEntityinfoComponent, data: { title: '用户实体管理', reuse: true } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SecurityRoutingModule { }
