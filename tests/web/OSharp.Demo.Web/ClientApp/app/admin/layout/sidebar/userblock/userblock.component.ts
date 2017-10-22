import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

import { UserblockService } from './userblock.service';
import { LoggingService } from '../../../../shared/services/logging.services';

@Component({
    selector: 'app-userblock',
    templateUrl: './userblock.component.html',
    styleUrls: ['./userblock.component.scss']
})
export class UserblockComponent implements OnInit {
    user: any;
    constructor(private logger: LoggingService,
        public userblockService: UserblockService,
        public translate: TranslateService) {
        this.logger.info("admin-layout sidebar userblock ctor call");
        this.user = {
            picture: 'assets/img/user/01.jpg'
        };
    }

    ngOnInit() {
    }

    userBlockIsVisible() {
        return this.userblockService.getVisibility();
    }

}
