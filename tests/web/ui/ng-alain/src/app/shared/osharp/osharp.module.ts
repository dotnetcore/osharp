import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { KendouiService } from '@shared/osharp/services/kendoui.service';
import { RemoteValidator } from '@shared/osharp/directives/remote-validator.directive';

const DIRECTIVES = [
  RemoteValidator
];

const SERVICES = [
  OsharpService,
  KendouiService
];

@NgModule({
  declarations: [
    ...DIRECTIVES
  ],
  imports: [CommonModule],
  exports: [
    ...DIRECTIVES
  ],
  providers: [
    ...SERVICES
  ],
})
export class OsharpModule {

}
