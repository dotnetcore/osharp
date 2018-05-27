import { Component, OnInit, OnDestroy, } from '@angular/core';
import { RoleEntityinfoService, } from './role-entityinfo.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 'security-role-entityinfo',
    templateUrl: './role-entityinfo.component.html',
    styleUrls: ['./role-entityinfo.component.css']
})
export class RoleEntityinfoComponent implements OnInit, OnDestroy {

    constructor(private _service: RoleEntityinfoService) { }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
