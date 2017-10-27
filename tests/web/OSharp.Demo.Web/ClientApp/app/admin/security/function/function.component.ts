import { Component, OnInit, } from '@angular/core';
import { FunctionService, Function } from './function.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

import { LoggingService } from '../../../shared/services/logging.services';
import { DataSourceRequestState } from "@progress/kendo-data-query";
import { GridDataResult } from '@progress/kendo-angular-grid';

@Component({
    selector: 'app-function',
    templateUrl: './function.component.html',
    styleUrls: ['./function.component.scss']
})
export class FunctionComponent implements OnInit {

    title = "功能信息";
    public gridData: GridDataResult;
    public state: DataSourceRequestState = { skip: 0, take: 20 };

    constructor(private functionService: FunctionService) { }

    ngOnInit() {
        this.functionService.Read(this.state).subscribe(data => {
            this.gridData = data
        });
    }
}
