import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import { AdminRoutingModule } from './routes/admin.routing';
import { LayoutModule } from './layout/layout.module';
import { DashboardModule } from './dashboard/dashboard.module';

@NgModule({
    declarations: [],
    imports: [
        TranslateModule,
        AdminRoutingModule,
        LayoutModule,
        DashboardModule
    ],
    providers: []
})
export class AdminModule { }
