import { NgModule, } from '@angular/core';
import { RoutesModule } from './routes/routes.module';
import { LayoutModule } from '../../layout/layout.module';

import '@progress/kendo-ui';
import '@progress/kendo-ui/js/cultures/kendo.culture.zh-CN';
import '@progress/kendo-ui/js/messages/kendo.messages.zh-CN';

@NgModule({
  declarations: [
  ],
  imports: [
    RoutesModule,
    LayoutModule
  ],
  providers: [

  ]
})
export class AdminModule { }
