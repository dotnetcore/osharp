import { Component, OnInit } from '@angular/core';
import { SettingsService } from '../../shared/angle/core/settings/settings.service';

@Component({
    selector: '[app-footer]',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

    constructor(public settings: SettingsService) { }

    ngOnInit() {

    }

}
