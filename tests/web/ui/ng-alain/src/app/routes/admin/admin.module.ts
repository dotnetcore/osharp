import { NgModule, } from '@angular/core';

import '@progress/kendo-ui';
import '@progress/kendo-ui/js/cultures/kendo.culture.zh-CN';
import '@progress/kendo-ui/js/messages/kendo.messages.zh-CN';

import { LayoutModule } from '../../layout/layout.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { AdminRoutingModule } from './admin.routing';

@NgModule({
  declarations: [
  ],
  imports: [
    DashboardModule,
    LayoutModule,
    AdminRoutingModule
  ],
  providers: [

  ]
})
export class AdminModule { }
