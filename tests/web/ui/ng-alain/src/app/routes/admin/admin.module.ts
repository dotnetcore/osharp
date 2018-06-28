import { NgModule, } from '@angular/core';

import '@progress/kendo-ui';
import '@progress/kendo-ui/js/cultures/kendo.culture.zh-CN';
import '@progress/kendo-ui/js/messages/kendo.messages.zh-CN';

import { DashboardModule } from './dashboard/dashboard.module';
import { AdminRoutingModule } from './admin.routing';
import { AdminLayoutModule } from './layout/layout.module';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
  ],
  imports: [
    AdminLayoutModule,
    DashboardModule,
    AdminRoutingModule,
    SharedModule
  ],
  providers: [

  ]
})
export class AdminModule { }
