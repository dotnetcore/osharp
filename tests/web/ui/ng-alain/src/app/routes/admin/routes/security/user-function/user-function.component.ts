import { Component, OnInit, OnDestroy, } from '@angular/core';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
  selector: 'admin-security-user-function',
  templateUrl: './user-function.component.html',
})
export class UserFunctionComponent implements OnInit, OnDestroy {


  ngOnInit() {
  }

  ngOnDestroy() {
  }
}
