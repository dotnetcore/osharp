import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';

import { AdminRoutingModule, } from './admin.routing';
import { AngleModule } from "./angle/angle.module";
import { LayoutModule } from "./layout/layout.module";
import { HomeModule } from './home/home.module';
import { TranslatorService } from './angle/translator/translator.service';

@NgModule({
    declarations: [],
    imports: [
        TranslateModule,
        AdminRoutingModule,
        AngleModule,
        LayoutModule,
        HomeModule
    ],
    providers: [
        TranslatorService
    ]
})
export class AdminModule { }
