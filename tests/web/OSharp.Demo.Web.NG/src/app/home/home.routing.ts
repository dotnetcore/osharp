import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent, } from './home.component';

// TODO: import components
// import { DemoComponent } from './demo/demo.component';

const routes: Routes = [
    { path: '', component: HomeComponent, data: { title: '用户中心' }, children: [] }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HomeRoutingModule { }
