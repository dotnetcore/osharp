import { NgModule, Component, } from '@angular/core';
import { SystemRoutingModule, } from './system.routing';
import { SettingsComponent } from './settings/settings.component';
import { AuditOperationComponent } from './audit-operation/audit-operation.component';
import { AuditEntityComponent } from './audit-entity/audit-entity.component';

const COMPONENTS = [
  SettingsComponent,
  AuditOperationComponent,
  AuditEntityComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    SystemRoutingModule,
  ],
  providers: [
  ]
})
export class SystemModule { }
