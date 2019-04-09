import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { KendouiService } from '@shared/osharp/services/kendoui.service';
import { RemoteValidator } from '@shared/osharp/directives/remote-validator.directive';
import { KendouiFunctionComponent } from '@shared/osharp/components/kendoui-function.component';
import { KendouiSplitterComponent } from '@shared/osharp/components/kendoui-splitter.component';
import { KendouiTabstripComponent } from '@shared/osharp/components/kendoui-tabstrip.component';
import { KendouiTreeviewComponent } from '@shared/osharp/components/kendoui-treeview.component';
import { KendouiWindowComponent } from '@shared/osharp/components/kendoui-window.component';
import { KendouiComboBoxComponent } from "@shared/osharp/components/kendoui-combobox.component";
import { OsharpCacheModule } from '@shared/osharp/cache/cache.module';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { RemoteInverseValidator } from '@shared/osharp/directives/remote-inverse-validator.directive';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { FormsModule } from '@angular/forms';
import { AlainService } from './services/ng-alain.service';

const DIRECTIVES = [
  RemoteValidator,
  RemoteInverseValidator
];

const COMPONENTS = [
  KendouiFunctionComponent,
  KendouiSplitterComponent,
  KendouiTabstripComponent,
  KendouiTreeviewComponent,
  KendouiWindowComponent,
  KendouiComboBoxComponent,
];

const SERVICES = [
  OsharpService,
  KendouiService,
  IdentityService,
  AlainService,
];

@NgModule({
  declarations: [
    ...DIRECTIVES,
    ...COMPONENTS
  ],
  imports: [
    CommonModule,
    FormsModule,
    OsharpCacheModule.forRoot(),
    NgZorroAntdModule
  ],
  exports: [
    ...DIRECTIVES,
    OsharpCacheModule,
    ...COMPONENTS
  ],
  providers: [
    ...SERVICES
  ],
})
export class OsharpModule {

}
