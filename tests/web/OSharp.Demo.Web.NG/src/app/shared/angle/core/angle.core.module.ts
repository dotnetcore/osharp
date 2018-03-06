import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';

import { throwIfAlreadyLoaded } from "./module-import-guard";

import { SettingsService } from './settings/settings.service';
import { ThemesService } from './themes/themes.service';
import { TranslatorService } from './translator/translator.service';
import { MenuService } from './menu/menu.service';

@NgModule({
    declarations: [],
    imports: [CommonModule],
    exports: [],
    providers: [
        SettingsService,
        ThemesService,
        TranslatorService,
        MenuService
    ],
})
export class AngleCoreModule {
    constructor( @Optional() @SkipSelf() parentModule: AngleCoreModule) {
        throwIfAlreadyLoaded(parentModule, "AngleCoreModule");
    }

}