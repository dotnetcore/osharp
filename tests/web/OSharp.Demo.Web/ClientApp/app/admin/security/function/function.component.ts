import { Component, OnInit, } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { FunctionService, Function } from './function.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

import { DataSourceRequestState } from "@progress/kendo-data-query";
import { GridDataResult, DataStateChangeEvent } from '@progress/kendo-angular-grid';
import { LoggingService } from '../../../shared/services/logging.services';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
    selector: 'app-function',
    templateUrl: './function.component.html',
    styleUrls: ['./function.component.scss']
})
export class FunctionComponent implements OnInit {

    title = "功能信息管理";
    public gridData: GridDataResult;
    public model: Observable<GridDataResult>;
    public state: DataSourceRequestState = { skip: 0, take: 5 };

    constructor(private service: FunctionService) { }

    ngOnInit() {
        this.model = this.service.Read(this.state);
    }

    public refresh() {
        this.model = this.service.Read(this.state);
    }

    protected dataStateChange(state: DataStateChangeEvent): void {
        this.state = state;
        this.model = this.service.Read(this.state);
    }

    protected addRow({ sender }) {
        const row = new FormGroup({
            name: new FormControl("", Validators.required),
            accessType: new FormControl(0),
            cacheExpirationSeconds: new FormControl(0),
            auditOperationEnabled: new FormControl(false),
            auditEntityEnabled: new FormControl(false),
            isCacheSliding: new FormControl(false),
            isLocked: new FormControl(false),
            isAjax: new FormControl(false)
        });
        sender.addRow(row);
    }
}
