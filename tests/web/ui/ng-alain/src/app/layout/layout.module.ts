import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { LayoutModule as CdkLayoutModule } from '@angular/cdk/layout';
import { LayoutDefaultComponent } from './default/default.component';

const COMPONENTS = [
  LayoutDefaultComponent,

];

@NgModule({
  imports: [SharedModule, CdkLayoutModule],
  providers: [
  ],
  declarations: [
    ...COMPONENTS,
  ],
  exports: [
    ...COMPONENTS,
  ]
})
export class LayoutModule { }
