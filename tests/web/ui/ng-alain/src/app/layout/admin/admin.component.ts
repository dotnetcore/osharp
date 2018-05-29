import { Component } from '@angular/core';
import { Router, RouteConfigLoadStart, NavigationError, NavigationEnd } from '@angular/router';
import { ScrollService, MenuService, SettingsService } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'layout-admin',
  templateUrl: './admin.component.html'
})
export class LayoutAdminComponent {
  isFetching = false;

  constructor(
    router: Router,
    scroll: ScrollService,
    private _message: NzMessageService,
    public menuSrv: MenuService,
    public settings: SettingsService,
  ) {
    // scroll to top in change page
    router.events.subscribe(evt => {
      if (!this.isFetching && evt instanceof RouteConfigLoadStart) {
        this.isFetching = true;
      }
      if (evt instanceof NavigationError) {
        this.isFetching = false;
        console.log(evt);
        _message.error(`无法加载${evt.url}路由`, { nzDuration: 1000 * 3 });
        return;
      }
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      setTimeout(() => {
        scroll.scrollToTop();
        this.isFetching = false;
      }, 100);
    });
  }
}
