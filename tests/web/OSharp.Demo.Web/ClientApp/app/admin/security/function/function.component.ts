import { Component, OnInit, } from '@angular/core';
import { FunctionService, Function } from './function.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

import { PageData } from '../../../shared/osharp/osharp.models';
import { LoggingService } from '../../../shared/services/logging.services';

@Component({
    selector: 's-function',
    templateUrl: './function.component.html',
    styleUrls: ['./function.component.scss']
})
export class FunctionComponent implements OnInit {

    pageData: PageData<Function>;

    constructor(private functionService: FunctionService) { }

    ngOnInit() {
        this.functionService.Read().then(data => this.pageData = data);
    }
}
