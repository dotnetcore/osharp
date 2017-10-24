import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';
import { UserRoleComponent } from './user-role/user-role.component';

// TODO: import components
// import { DemoComponent } from './demo/demo.component';

const routes: Routes = [
    { path: 'user', component: UserComponent },
    { path: 'role', component: RoleComponent },
    { path: 'user-role', component: UserRoleComponent },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class IdentityRoutingModule { }
