import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { TitleService } from '@delon/theme';
import { filter } from 'rxjs/operators';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
  selector: 'app-root',
  template: `
  <router-outlet></router-outlet>
  `,
})
export class AppComponent {
  isFetching = false;

  constructor(
    router: Router,
    private titleSrv: TitleService
  ) {
    router.events
      .pipe(filter(evt => evt instanceof NavigationEnd))
      .subscribe(() => {
        this.titleSrv.setTitle();
      });
  }
}
