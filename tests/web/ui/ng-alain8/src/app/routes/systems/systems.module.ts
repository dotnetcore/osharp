import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SystemsRoutingModule } from './systems.routing';
import { AuditEntityComponent } from './audit-entity/audit-entity.component';
import { AuditOperationComponent } from './audit-operation/audit-operation.component';
import { DataDictionaryComponent } from './data-dictionary/data-dictionary.component';
import { PackComponent } from './pack/pack.component';
import { SettingsComponent } from './settings/settings.component';


@NgModule({
  declarations: [AuditEntityComponent, AuditOperationComponent, DataDictionaryComponent, PackComponent, SettingsComponent],
  imports: [
    CommonModule,
    SystemsRoutingModule
  ]
})
export class SystemsModule { }
