import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';

import { SystemRoutingModule, } from './system.routing';
import { SettingsComponent } from './settings/settings.component';
import { SettingsService } from './settings/settings.service';

@NgModule({
    declarations: [
        SettingsComponent
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
