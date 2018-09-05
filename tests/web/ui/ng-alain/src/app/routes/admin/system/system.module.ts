import { NgModule, Component, } from '@angular/core';
import { SystemRoutingModule, } from './system.routing';
import { SharedModule } from '@shared/shared.module';
import { SettingsComponent } from './settings/settings.component';
import { AuditOperationComponent } from './audit-operation/audit-operation.component';
import { AuditEntityComponent } from './audit-entity/audit-entity.component';
import { PackComponent } from './pack/pack.component';

const COMPONENTS = [
  SettingsComponent,
  AuditOperationComponent,
  AuditEntityComponent,
  PackComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    SystemRoutingModule,
    SharedModule
  ],
  providers: [
  ]
})
export class SystemModule { }
