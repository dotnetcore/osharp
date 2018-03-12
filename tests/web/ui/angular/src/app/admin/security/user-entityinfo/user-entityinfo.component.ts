import { Component, OnInit, OnDestroy, } from '@angular/core';
import { UserEntityinfoService, } from './user-entityinfo.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 'security-user-entityinfo',
    templateUrl: './user-entityinfo.component.html',
    styleUrls: ['./user-entityinfo.component.css']
})
export class UserEntityinfoComponent implements OnInit, OnDestroy {

    constructor(private _service: UserEntityinfoService) { }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
