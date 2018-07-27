import { NgModule, } from '@angular/core';
import { DashboardComponent } from './dashboard.component';
import { SharedModule } from '@shared/shared.module';

import '@antv/g2';
import '@antv/g2-plugin-slider';
import '@antv/data-set';


@NgModule({
  declarations: [
    DashboardComponent,
  ],
  imports: [
    SharedModule
  ],
  providers: [
  ]
})
export class DashboardModule { }
