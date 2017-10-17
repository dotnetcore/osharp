import { Component, } from '@angular/core';
import { LoggingService } from "../../shared/services/logging.services";

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['layout.component.scss']
})
export class LayoutComponent {
    constructor(logger: LoggingService) {
        logger.info("admin-layout ctor call");
    }
}
