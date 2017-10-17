import { Component, } from '@angular/core';
import { LoggingService } from '../../shared/services/logging.services';

@Component({
    selector: 's-home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    constructor(private logger: LoggingService) {
        logger.info("admin-home ctor call");
    }
}
