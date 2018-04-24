import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import '@progress/kendo-ui'
import '@progress/kendo-ui/js/cultures/kendo.culture.zh-CN'
import '@progress/kendo-ui/js/messages/kendo.messages.zh-CN'
import { kendoui } from "../shared/kendoui";
import { osharp } from "../shared/osharp";

import { AdminRoutingModule } from './routes/admin.routing';
import { LayoutModule } from './layout/layout.module';
import { DashboardModule } from './dashboard/dashboard.module';

@NgModule({
  declarations: [],
  imports: [
    TranslateModule,
    AdminRoutingModule,
    LayoutModule,
    DashboardModule
  ],
  providers: []
})
export class AdminModule { }
