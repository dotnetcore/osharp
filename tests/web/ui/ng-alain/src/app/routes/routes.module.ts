import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';
import { AppRoutingModule } from './app.routing';
import { HomeModule } from "./home/home.module";
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// single pages
import { CallbackComponent } from './callback/callback.component';
import { Exception403Component } from './exception/403.component';
import { Exception404Component } from './exception/404.component';
import { Exception500Component } from './exception/500.component';

const COMPONENTS = [
  DashboardComponent,
  // single pages
  CallbackComponent,
  Exception403Component,
  Exception404Component,
  Exception500Component
];
const COMPONENTS_NOROUNT = [];

@NgModule({
  imports: [
    SharedModule,
    AppRoutingModule,
    HomeModule
  ],
  declarations: [
    ...COMPONENTS,
    ...COMPONENTS_NOROUNT
  ],
  entryComponents: COMPONENTS_NOROUNT
})
export class RoutesModule { }
