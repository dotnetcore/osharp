import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import { LayoutDefaultComponent } from './default/default.component';
import { DefaultHeaderComponent } from './default/header/header.component';


const COMPONENTS = [
  DefaultHeaderComponent,
  LayoutDefaultComponent,
];

@NgModule({
  imports: [SharedModule],
  providers: [],
  declarations: [
    ...COMPONENTS,
  ],
  exports: [
    ...COMPONENTS,
  ]
})
export class LayoutModule { }
