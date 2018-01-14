import { Component, } from '@angular/core';
import { LoggingService } from '../../shared/services/logging.services';

@Component({
    selector: 's-dashboard',
    templateUrl: './dashboard.component.html'
})
export class DashboardComponent {
    constructor(logger: LoggingService) {
        logger.info("admin-dashboard ctor call");
    }
}
