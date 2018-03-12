import { Component, OnInit, OnDestroy, } from '@angular/core';
import { RoleService, } from './role.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 'identity-role',
    templateUrl: './role.component.html',
    styleUrls: ['./role.component.css']
})
export class RoleComponent implements OnInit, OnDestroy {

    constructor(private _service: RoleService) { }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
