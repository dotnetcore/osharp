import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { SystemsRoutingModule } from './systems.routing';
import { AuditEntityComponent } from './audit-entity/audit-entity.component';
import { AuditOperationComponent } from './audit-operation/audit-operation.component';
import { PackComponent } from './pack/pack.component';
import { SettingsComponent } from './settings/settings.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    SystemsRoutingModule
  ],
  declarations: [
    AuditEntityComponent,
    AuditOperationComponent,
    PackComponent,
    SettingsComponent,
  ]
})
export class SystemsModule { }
