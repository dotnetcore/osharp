import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';
import { UserRoleComponent } from './user-role/user-role.component';

const routes: Routes = [
  { path: 'user', component: UserComponent, data: { title: '用户信息管理', reuse: true, titleI18n: "menu.nav.permission.identity.user" } },
  { path: 'role', component: RoleComponent, data: { title: '角色信息管理', reuse: true, titleI18n: "menu.nav.permission.identity.role" } },
  { path: 'user-role', component: UserRoleComponent, data: { title: '用户角色管理', reuse: true, titleI18n: "menu.nav.permission.identity.user-role" } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityRoutingModule { }
