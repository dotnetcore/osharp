import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsService } from "./settings/settings.service";
import { TranslatorService } from './translator/translator.service';

@NgModule({
    declarations: [],
    imports: [CommonModule],
    exports: [],
    providers: [
        SettingsService,
        TranslatorService
    ],
})
export class AngleModule { }