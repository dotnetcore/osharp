import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminLayoutComponent } from './layout.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { SharedModule } from '@shared/shared.module';
import { LayoutModule } from '../../../layout/layout.module';

const COMPONENTS = [
  AdminLayoutComponent,

  SidebarComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    CommonModule,
    SharedModule,
    LayoutModule
  ],
  exports: [],
  providers: [],
})
export class AdminLayoutModule { }
