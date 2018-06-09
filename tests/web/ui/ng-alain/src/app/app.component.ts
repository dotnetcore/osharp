import { Component } from '@angular/core';
import { Router, NavigationEnd, RouteConfigLoadStart, NavigationError } from '@angular/router';
import { SettingsService, TitleService, ScrollService } from '@delon/theme';
import { filter } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';

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
      .subscribe(evt => {
        this.titleSrv.setTitle();
      });
  }
}
