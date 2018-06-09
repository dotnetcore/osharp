import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import { LayoutDefaultComponent } from './default/default.component';
import { LayoutFullScreenComponent } from './fullscreen/fullscreen.component';
import { LayoutPassportComponent } from './passport/passport.component';
import { DefaultHeaderComponent } from './default/header/header.component';


const COMPONENTS = [
  DefaultHeaderComponent,
  LayoutDefaultComponent,
  LayoutFullScreenComponent,
];

// passport
const PASSPORT = [
  LayoutPassportComponent
];

@NgModule({
  imports: [SharedModule],
  providers: [],
  declarations: [
    ...COMPONENTS,
    ...PASSPORT
  ],
  exports: [
    ...COMPONENTS,
    ...PASSPORT
  ]
})
export class LayoutModule { }
