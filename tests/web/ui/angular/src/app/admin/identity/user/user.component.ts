import { Component, OnInit, OnDestroy, } from '@angular/core';
import { UserService, } from './user.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 'identity-user',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit, OnDestroy {

    constructor(private _service: UserService) { }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
