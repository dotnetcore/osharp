import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SettingsComponent } from './settings/settings.component';
import { AuditOperationComponent } from './audit-operation/audit-operation.component';
import { AuditEntityComponent } from './audit-entity/audit-entity.component';
import { PackComponent } from './pack/pack.component';

const routes: Routes = [
  { path: 'settings', component: SettingsComponent, data: { title: '系统设置', reuse: true } },
  { path: 'audit-operation', component: AuditOperationComponent, data: { title: '操作审计', reuse: true } },
  { path: 'audit-entity', component: AuditEntityComponent, data: { title: '数据审计', reuse: true } },
  { path: 'pack', component: PackComponent, data: { title: '模块包', reuse: true } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule { }
