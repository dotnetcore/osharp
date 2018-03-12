import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SettingsComponent } from './settings/settings.component';


const routes: Routes = [
    { path: 'settings', component: SettingsComponent, data: { title: '系统设置 - 管理' } }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SystemRoutingModule { }
