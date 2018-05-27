import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';
import { UserRoleComponent } from './user-role/user-role.component';


// TODO: import components
// import { DemoComponent } from './demo/demo.component';

const routes: Routes = [
    { path: 'user', component: UserComponent, data: { title: '用户信息 - 管理' } },
    { path: 'role', component: RoleComponent, data: { title: '角色信息 - 管理' } },
    { path: 'user-role', component: UserRoleComponent, data: { title: '用户角色信息 - 管理' } },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class IdentityRoutingModule { }
