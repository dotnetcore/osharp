import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';
import { OsharpService } from './osharp.service';
import { OsharpSettingsService } from './osharp.settings.service';

@NgModule({
  declarations: [
  ],
  imports: [
    TranslateModule,
  ],
  exports: [
  ],
  providers: [
    OsharpService,
    OsharpSettingsService
  ]
})
export class OsharpModule { }
