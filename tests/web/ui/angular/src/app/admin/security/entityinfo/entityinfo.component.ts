import { Component, OnInit, OnDestroy, } from '@angular/core';
import { EntityinfoService, } from './entityinfo.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 'security-entityinfo',
    templateUrl: './entityinfo.component.html',
    styleUrls: ['./entityinfo.component.css']
})
export class EntityinfoComponent implements OnInit, OnDestroy {

    constructor(private _service: EntityinfoService) { }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
