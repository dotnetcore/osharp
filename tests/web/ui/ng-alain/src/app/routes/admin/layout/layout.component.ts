import { Component, HostBinding, } from '@angular/core';
import { SettingsService, ScrollService, MenuService } from '@delon/theme';
import { Router, RouteConfigLoadStart, NavigationError, NavigationEnd } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '@shared/osharp/services/identity.service';

@Component({
  selector: 'admin-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class AdminLayoutComponent {
  isFetching = false;

  @HostBinding('class.layout-fixed')
  get isFixed() {
    return this.settings.layout.fixed;
  }
  @HostBinding('class.layout-boxed')
  get isBoxed() {
    return this.settings.layout.boxed;
  }
  @HostBinding('class.aside-collapsed')
  get isCollapsed() {
    return this.settings.layout.collapsed;
  }

  constructor(
    router: Router,
    scroll: ScrollService,
    _message: NzMessageService,
    public menuSrv: MenuService,
    public settings: SettingsService,
  ) {
    if (!settings.user.isAdmin) {
      _message.error("你无权查看后台管理页面，即将跳转到首页");
      setTimeout(() => {
        router.navigate(['home']);
      }, 100);
      return;
    }
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
