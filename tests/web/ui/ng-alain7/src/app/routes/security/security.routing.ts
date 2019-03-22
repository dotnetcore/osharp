import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ModuleComponent } from './module/module.component';
import { FunctionComponent } from './function/function.component';
import { RoleFunctionComponent } from './role-function/role-function.component';
import { UserFunctionComponent } from './user-function/user-function.component';
import { EntityinfoComponent } from './entityinfo/entityinfo.component';
import { RoleEntityinfoComponent } from './role-entityinfo/role-entityinfo.component';
import { ACLGuard } from '@delon/acl';

const routes: Routes = [
  { path: 'module', component: ModuleComponent, canActivate: [ACLGuard], data: { title: '模块信息管理', reuse: true, titleI18n: "menu.nav.permission.security.module", guard: 'Root.Admin.Security.Module.Read' } },
  { path: 'function', component: FunctionComponent, canActivate: [ACLGuard], data: { title: '功能信息管理', reuse: true, titleI18n: "menu.nav.permission.security.function", guard: 'Root.Admin.Security.Function.Read' } },
  { path: 'role-function', component: RoleFunctionComponent, canActivate: [ACLGuard], data: { title: '角色功能管理', reuse: true, titleI18n: "menu.nav.permission.security.role-function", guard: 'Root.Admin.Security.RoleFunction.Read' } },
  { path: 'user-function', component: UserFunctionComponent, canActivate: [ACLGuard], data: { title: '用户功能管理', reuse: true, titleI18n: "menu.nav.permission.security.user-function", guard: 'Root.Admin.Security.UserFunction.Read' } },
  { path: 'entityinfo', component: EntityinfoComponent, canActivate: [ACLGuard], data: { title: '实体管理', reuse: true, titleI18n: "menu.nav.permission.security.entityinfo", guard: 'Root.Admin.Security.EntityInfo.Read' } },
  { path: 'role-entityinfo', component: RoleEntityinfoComponent, canActivate: [ACLGuard], data: { title: '角色数据管理', reuse: true, titleI18n: "menu.nav.permission.security.role-entityinfo", guard: 'Root.Admin.Security.RoleEntity.Read' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SecurityRoutingModule { }
