import { Component, OnInit, OnDestroy, } from '@angular/core';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class DefaultHeaderComponent implements OnInit, OnDestroy {


  ngOnInit() {
  }

  ngOnDestroy() {
  }
}
