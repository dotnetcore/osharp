import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ACLGuard } from '@delon/acl';

import { ModuleComponent } from './module/module.component';
import { FunctionComponent } from './function/function.component';
import { EntityinfoComponent } from './entityinfo/entityinfo.component';
import { RoleEntityinfoComponent } from './role-entityinfo/role-entityinfo.component';


const routes: Routes = [
  { path: 'module', component: ModuleComponent, canActivate: [ACLGuard], data: { title: '模块信息管理', reuse: true, titleI18n: "menu.nav.permission.security.module", guard: 'Root.Admin.Security.Module.Read' } },
  { path: 'function', component: FunctionComponent, canActivate: [ACLGuard], data: { title: '功能信息管理', reuse: true, titleI18n: "menu.nav.permission.security.function", guard: 'Root.Admin.Security.Function.Read' } },
  { path: 'entityinfo', component: EntityinfoComponent, canActivate: [ACLGuard], data: { title: '数据信息管理', reuse: true, titleI18n: "menu.nav.permission.security.entityinfo", guard: 'Root.Admin.Security.EntityInfo.Read' } },
  { path: 'role-entityinfo', component: RoleEntityinfoComponent, canActivate: [ACLGuard], data: { title: '数据权限管理', reuse: true, titleI18n: "menu.nav.permission.security.role-entityinfo", guard: 'Root.Admin.Security.RoleEntity.Read' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SecurityRoutingModule { }
