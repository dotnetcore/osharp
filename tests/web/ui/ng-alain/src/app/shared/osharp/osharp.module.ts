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

const DIRECTIVES = [
  RemoteValidator
];

const COMPONENTS = [
  KendouiFunctionComponent,
  KendouiSplitterComponent,
  KendouiTabstripComponent,
  KendouiTreeviewComponent,
  KendouiWindowComponent,
];

const SERVICES = [
  OsharpService,
  KendouiService,
];

@NgModule({
  declarations: [
    ...DIRECTIVES,
    ...COMPONENTS
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    ...DIRECTIVES,
    ...COMPONENTS
  ],
  providers: [
    ...SERVICES
  ],
})
export class OsharpModule {

}
