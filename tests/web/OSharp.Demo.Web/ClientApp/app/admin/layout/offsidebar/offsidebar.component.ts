import { Component, } from '@angular/core';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

import { LoggingService } from '../../../shared/services/logging.services';
import { SettingsService } from "../../../shared/angle/core/settings/settings.service";
import { ThemesService } from "../../../shared/angle/core/themes/themes.service";
import { TranslatorService } from "../../../shared/angle/core/translator/translator.service";

@Component({
    selector: 'app-offsidebar',
    templateUrl: './offsidebar.component.html',
    styleUrls: ['./offsidebar.component.scss']
})
export class OffsidebarComponent {

    currentTheme: string;
    selectedLang: string;

    constructor(private logger: LoggingService,
        public themes: ThemesService,
        public settings: SettingsService,
        public translator: TranslatorService
    ) {
        logger.info("admin-layout offsidebar ctor call");

        this.currentTheme = themes.getDefaultTheme();
        this.selectedLang = this.getLangs()[0].code;
    }

    setTheme() {
        this.themes.setTheme(this.currentTheme);
    }

    getLangs() {
        return this.translator.getAvailableLanguages();
    }

    setLang(value) {
        this.translator.useLanguage(value);
    }
}
