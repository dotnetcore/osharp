import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { SystemsRoutingModule } from './systems.routing';
import { AuditEntityComponent } from './audit-entity/audit-entity.component';
import { AuditOperationComponent } from './audit-operation/audit-operation.component';
import { PackComponent } from './pack/pack.component';
import { SettingsComponent } from './settings/settings.component';
import { DataDictionaryComponent } from './data-dictionary/data-dictionary.component';

import '@progress/kendo-ui/js/kendo.web.js';
import '@progress/kendo-ui/js/cultures/kendo.culture.zh-CN';
import '@progress/kendo-ui/js/messages/kendo.messages.zh-CN';

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
    DataDictionaryComponent,
  ]
})
export class SystemsModule { }
