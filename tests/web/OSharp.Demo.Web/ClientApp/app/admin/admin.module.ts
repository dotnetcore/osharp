import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';

import { AdminRoutingModule } from "./routing/admin.routing";
import { LayoutModule } from "./layout/layout.module";
import { HomeModule } from './home/home.module';

@NgModule({
    declarations: [],
    imports: [
        TranslateModule,
        AdminRoutingModule,
        LayoutModule,
        HomeModule
    ],
    providers: []
})
export class AdminModule { }
