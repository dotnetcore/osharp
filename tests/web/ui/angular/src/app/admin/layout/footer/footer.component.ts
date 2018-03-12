import { Component, OnInit, OnDestroy, } from '@angular/core';

import { SettingsService } from '../../../shared/angle/core/settings/settings.service';

@Component({
    selector: 'admin-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit, OnDestroy {

    constructor(public settings: SettingsService) { }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
