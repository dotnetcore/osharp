import { Component, OnInit, OnDestroy, } from '@angular/core';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

import { LoggingService } from '../../../shared/services/logging.services';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

    constructor(private logger: LoggingService) {
        logger.info("admin-layout header ctor call");
    }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
