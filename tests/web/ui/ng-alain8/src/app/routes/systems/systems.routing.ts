import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ACLGuard } from '@delon/acl';

import { SettingsComponent } from './settings/settings.component';
import { DataDictionaryComponent } from './data-dictionary/data-dictionary.component';
import { AuditOperationComponent } from './audit-operation/audit-operation.component';
import { AuditEntityComponent } from './audit-entity/audit-entity.component';
import { PackComponent } from './pack/pack.component';


const routes: Routes = [
  { path: 'settings', component: SettingsComponent, canActivate: [ACLGuard], data: { title: '系统设置', reuse: true, titleI18n: "menu.nav.system.systems.settings" } },
  { path: 'data-dictionary', component: DataDictionaryComponent, canActivate: [ACLGuard], data: { title: '数据字典', reuse: true, titleI18n: "menu.nav.system.systems.data-dictionary" } },
  { path: 'audit-operation', component: AuditOperationComponent, canActivate: [ACLGuard], data: { title: '操作审计', reuse: true, titleI18n: "menu.nav.system.systems.audit-operation" } },
  { path: 'audit-entity', component: AuditEntityComponent, canActivate: [ACLGuard], data: { title: '数据审计', reuse: true, titleI18n: "menu.nav.system.systems.audit-entity" } },
  { path: 'pack', component: PackComponent, canActivate: [ACLGuard], data: { title: '模块包', reuse: true, titleI18n: "menu.nav.system.systems.pack" } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemsRoutingModule { }
