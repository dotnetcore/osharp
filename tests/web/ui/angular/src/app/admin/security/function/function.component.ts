import { Component, OnInit, OnDestroy, } from '@angular/core';
import { FunctionService, } from './function.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 'security-function',
    templateUrl: './function.component.html',
    styleUrls: ['./function.component.css']
})
export class FunctionComponent implements OnInit, OnDestroy {

    constructor(private _service: FunctionService) { }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
