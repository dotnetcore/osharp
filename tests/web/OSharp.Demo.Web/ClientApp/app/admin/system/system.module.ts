import { NgModule, } from '@angular/core';
import { SystemRoutingModule, } from './system.routing';
import { TranslateModule, } from '@ngx-translate/core';

import { SettingsComponent } from './settings/settings.component';
import { SettingsService } from './settings/settings.service';

@NgModule({
    declarations: [
        SettingsComponent,

    ],
    imports: [
        TranslateModule,
        SystemRoutingModule,
    ],
    providers: [
        SettingsService
    ]
})
export class SystemModule { }
