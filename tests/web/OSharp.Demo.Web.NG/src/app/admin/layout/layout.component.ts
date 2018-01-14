import { Component, OnInit } from '@angular/core';
declare var $: any;

import { LoggingService } from '../../shared/services/logging.services';

@Component({
    selector: 's-layout',
    templateUrl: './layout.component.html'
})
export class LayoutComponent implements OnInit {
    constructor(logger: LoggingService) {
        logger.info("admin-layout ctor call")
    }

    ngOnInit() {
        $(document).on('click', '[href="#"]', e => e.preventDefault());
    }
}
