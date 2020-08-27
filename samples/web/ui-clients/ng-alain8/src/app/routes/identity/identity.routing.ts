import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ACLGuard } from '@delon/acl';

import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';
import { UserRoleComponent } from './user-role/user-role.component';


const routes: Routes = [
  { path: 'user', component: UserComponent, canActivate: [ACLGuard], data: { title: '用户信息管理', reuse: true, titleI18n: "menu.nav.permission.identity.user", guard: 'Root.Admin.Identity.User.Read' } },
  { path: 'role', component: RoleComponent, canActivate: [ACLGuard], data: { title: '角色信息管理', reuse: true, titleI18n: "menu.nav.permission.identity.role", guard: 'Root.Admin.Identity.Role.Read' } },
  { path: 'user-role', component: UserRoleComponent, canActivate: [ACLGuard], data: { title: '用户角色管理', reuse: true, titleI18n: "menu.nav.permission.identity.user-role", guard: 'Root.Admin.Identity.UserRole.Read' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityRoutingModule { }
