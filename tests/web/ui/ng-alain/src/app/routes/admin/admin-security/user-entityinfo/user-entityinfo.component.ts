import { Component, OnInit, OnDestroy, } from '@angular/core';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
  selector: 'admin-security-user-entityinfo',
  templateUrl: './user-entityinfo.component.html',
})
export class UserEntityinfoComponent implements OnInit, OnDestroy {


  ngOnInit() {
  }

  ngOnDestroy() {
  }
}
