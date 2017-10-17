import { Component, } from '@angular/core';
import { LoggingService } from "../shared/services/logging.services";

@Component({
    selector: 's-demo01',
    templateUrl: './demo01.component.html'
})
export class Demo01Component {
    constructor(logger: LoggingService) {
        logger.info("demo01 ctor call");
    }

}
