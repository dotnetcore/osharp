import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminLayoutComponent } from './layout.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { SharedModule } from '@shared/shared.module';
import { HeaderComponent } from './header/header.component';
import { HeaderFullScreenComponent } from './header/components/fullscreen.component';
import { HeaderIconComponent } from './header/components/icon.component';
import { HeaderNotifyComponent } from './header/components/notify.component';
import { HeaderStorageComponent } from './header/components/storage.component';
import { HeaderUserComponent } from './header/components/user.component';

const COMPONENTS = [
  AdminLayoutComponent,
  SidebarComponent,
  HeaderComponent,
  HeaderFullScreenComponent,
  HeaderIconComponent,
  HeaderNotifyComponent,
  HeaderStorageComponent,
  HeaderUserComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    CommonModule,
    SharedModule,
  ],
  exports: [],
  providers: [],
})
export class AdminLayoutModule { }
