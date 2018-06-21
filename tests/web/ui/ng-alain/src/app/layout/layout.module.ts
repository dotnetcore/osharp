import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { LayoutModule as CdkLayoutModule } from '@angular/cdk/layout';
import { LayoutDefaultComponent } from './default/default.component';
import { HeaderComponent } from './default/header/header.component';
import { HeaderFullScreenComponent } from './default/header/components/fullscreen.component';
import { HeaderIconComponent } from './default/header/components/icon.component';
import { HeaderNotifyComponent } from './default/header/components/notify.component';
import { HeaderStorageComponent } from './default/header/components/storage.component';
import { HeaderUserComponent } from './default/header/components/user.component';

const COMPONENTS = [
  LayoutDefaultComponent,

  // header
  HeaderComponent,
  HeaderFullScreenComponent,
  HeaderIconComponent,
  HeaderNotifyComponent,
  HeaderStorageComponent,
  HeaderUserComponent,
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
