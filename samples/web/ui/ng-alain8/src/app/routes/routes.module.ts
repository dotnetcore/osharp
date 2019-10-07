import { NgModule } from '@angular/core';

import { SharedModule } from '@shared';
import { RouteRoutingModule } from './routes.routing';
import { DelonChartModule } from '@delon/chart';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// single pages
import { CallbackComponent } from './callback/callback.component';

const COMPONENTS = [
  DashboardComponent,
  // single pages
  CallbackComponent,
];
const COMPONENTS_NOROUNT = [];

@NgModule({
  imports: [SharedModule, RouteRoutingModule, DelonChartModule],
  declarations: [
    ...COMPONENTS,
    ...COMPONENTS_NOROUNT
  ],
  entryComponents: COMPONENTS_NOROUNT
})
export class RoutesModule { }
