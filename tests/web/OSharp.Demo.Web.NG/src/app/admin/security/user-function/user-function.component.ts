import { Component, OnInit, OnDestroy, } from '@angular/core';
import { UserFunctionService, } from './user-function.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 's-user-function',
    templateUrl: './user-function.component.html',
    styleUrls: ['./user-function.component.scss']
})
export class UserFunctionComponent implements OnInit, OnDestroy {

    constructor(private _service: UserFunctionService) { }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
