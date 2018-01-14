import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent, } from './users.component';

// TODO: import components
// import { DemoComponent } from './demo/demo.component';

const routes: Routes = [
    { path: '', component: UsersComponent, data: { title: '用户中心' }, children: [] }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UsersRoutingModule { }
