import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';

import { AdminRoutingModule, } from './routing/admin.routing';
import { MenuService } from '../shared/angle/core/menu/menu.service';
import { LayoutModule } from "./layout/layout.module";
import { DashboardModule } from "./dashboard/dashboard.module";

@NgModule({
    declarations: [],
    imports: [
        TranslateModule,
        AdminRoutingModule,
        LayoutModule,
        DashboardModule
    ],
    providers: [
        MenuService
    ]
})
export class AdminModule { }
