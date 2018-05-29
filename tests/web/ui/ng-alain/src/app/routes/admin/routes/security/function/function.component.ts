import { Component, OnInit, OnDestroy, } from '@angular/core';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
  selector: 'admin-security-function',
  templateUrl: './function.component.html',
})
export class FunctionComponent implements OnInit, OnDestroy {

  ngOnInit() {
  }

  ngOnDestroy() {
  }
}
