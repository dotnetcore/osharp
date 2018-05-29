import { Component, OnInit, OnDestroy, } from '@angular/core';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
  selector: 'admin-identity-user-role',
  templateUrl: './user-role.component.html'
})
export class UserRoleComponent implements OnInit, OnDestroy {

  ngOnInit() {
  }

  ngOnDestroy() {
  }
}
