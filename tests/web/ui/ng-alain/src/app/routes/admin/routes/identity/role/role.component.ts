import { Component, OnInit, OnDestroy, } from '@angular/core';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
  selector: 'admin-identity-role',
  templateUrl: './role.component.html'
})
export class RoleComponent implements OnInit, OnDestroy {

  ngOnInit() {
  }

  ngOnDestroy() {
  }
}
