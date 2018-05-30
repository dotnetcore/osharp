import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import { LayoutDefaultComponent } from './default/default.component';
import { LayoutFullScreenComponent } from './fullscreen/fullscreen.component';
import { LayoutAdminComponent } from "./admin/admin.component";
import { AdminHeaderComponent } from "./admin/header/header.component";
import { AdminSidebarComponent } from "./admin/sidebar/sidebar.component";
import { LayoutPassportComponent } from './passport/passport.component';
import { HeaderNotifyComponent } from './admin/header/components/notify.component';
import { HeaderIconComponent } from './admin/header/components/icon.component';
import { HeaderUserComponent } from './admin/header/components/user.component';
import { HeaderFullScreenComponent } from './admin/header/components/fullscreen.component';
import { HeaderStorageComponent } from './admin/header/components/storage.component';


const COMPONENTS = [
  LayoutDefaultComponent,
  LayoutFullScreenComponent,
  LayoutAdminComponent,
  AdminHeaderComponent,
  AdminSidebarComponent
];

const HEADERCOMPONENTS = [
  HeaderNotifyComponent,
  HeaderIconComponent,
  HeaderUserComponent,
  HeaderFullScreenComponent,
  HeaderStorageComponent
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
    ...HEADERCOMPONENTS,
    ...PASSPORT
  ],
  exports: [
    ...COMPONENTS,
    ...PASSPORT
  ]
})
export class LayoutModule { }
