import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { LayoutModule as CdkLayoutModule } from '@angular/cdk/layout';
import { LayoutDefaultComponent } from './default/default.component';
import { UserMenuComponent } from './default/components/user-menu/user-menu.component';
import { FooterComponent } from './default/components/footer/footer.component';


const COMPONENTS = [
  LayoutDefaultComponent,
  UserMenuComponent,
  FooterComponent
];

@NgModule({
  imports: [
    SharedModule,
    CdkLayoutModule,
  ],
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
