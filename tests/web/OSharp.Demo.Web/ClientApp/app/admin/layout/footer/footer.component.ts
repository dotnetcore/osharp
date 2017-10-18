import { Component, OnInit, OnDestroy, } from '@angular/core';
import { SettingsService } from "../../angle/settings/settings.service";
import { LoggingService } from '../../../shared/services/logging.services';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit, OnDestroy {

    constructor(
        private settings: SettingsService,
        private logger: LoggingService) {
        logger.info("admin-layout footer ctor call");
    }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
