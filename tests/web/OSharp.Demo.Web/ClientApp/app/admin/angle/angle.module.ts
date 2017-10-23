import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsService } from "./settings/settings.service";
import { TranslatorService } from './translator/translator.service';
import { MenuService } from './menu/menu.service';

@NgModule({
    declarations: [],
    imports: [CommonModule],
    exports: [],
    providers: [
        SettingsService,
        TranslatorService,
        MenuService
    ],
})
export class AngleModule { }