import { Component, OnInit, OnDestroy, } from '@angular/core';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

import { LoggingService } from '../../../shared/services/logging.services';

@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit, OnDestroy {

    constructor(private logger: LoggingService) {
        logger.info("admin-layout sidebar ctor call");
    }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
