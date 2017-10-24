import { Component, OnInit, OnDestroy, } from '@angular/core';
import { RoleFunctionService, } from './role-function.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 's-role-function',
    templateUrl: './role-function.component.html',
    styleUrls: ['./role-function.component.css']
})
export class RoleFunctionComponent implements OnInit, OnDestroy {

    constructor(private _service: RoleFunctionService) { }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
